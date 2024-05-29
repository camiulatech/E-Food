using NUnit.Framework;
using EFood.AccesoDatos.Repositorio.IRepositorio;
using E_Food.Areas.Admin.Controllers;
using EFood.Modelos;
using EFood.Modelos.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Moq;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.IO;
using System.Security.Claims;
using Microsoft.Extensions.Primitives;


namespace E_Food.Tests
{
    [TestFixture]
    public class ProductoControllerTests
    {
        private ProductoController _controller;
        private Mock<IUnidadTrabajo> _unidadTrabajoMock;
        private Mock<IWebHostEnvironment> _webHostEnvironmentMock;

        [SetUp]
        public void SetUp()
        {
            _unidadTrabajoMock = new Mock<IUnidadTrabajo>();
            _webHostEnvironmentMock = new Mock<IWebHostEnvironment>();

            // Configurar el contexto del controlador para simular la autenticación del usuario
            _controller = new ProductoController(_unidadTrabajoMock.Object, _webHostEnvironmentMock.Object)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = new DefaultHttpContext
                    {
                        User = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
                        {
                    new Claim(ClaimTypes.Name, "testuser") // Nombre del usuario autenticado
                                                           // Puedes agregar más reclamaciones según sea necesario para simular roles, etc.
                        }, "mock"))
                    }
                }
            };
        }


        [TearDown]
        public void TearDown()
        {
            _controller.Dispose();
        }

        [Test]
        public void Index_ReturnsViewResult()
        {
            // Act
            var result = _controller.Index();

            // Assert
            Assert.IsInstanceOf<ViewResult>(result);
        }

        [Test]
        public async Task Upsert_WithValidId_ReturnsViewResultWithProductoVM()
        {
            // Arrange
            int validId = 1;
            var productoVM = new ProductoVM();
            _unidadTrabajoMock.Setup(u => u.Producto.Obtener(validId)).ReturnsAsync(new Producto());

            // Act
            var result = await _controller.Upsert(validId);

            // Assert
            Assert.IsInstanceOf<ViewResult>(result);
            var viewResult = result as ViewResult;
            Assert.IsInstanceOf<ProductoVM>(viewResult.Model);
        }

        [Test]
        public async Task Upsert_WithInvalidId_ReturnsNotFoundResult()
        {
            // Arrange
            int invalidId = 999;
            _unidadTrabajoMock.Setup(u => u.Producto.Obtener(invalidId)).ReturnsAsync((Producto)null);

            // Act
            var result = await _controller.Upsert(invalidId);

            // Assert
            Assert.IsInstanceOf<NotFoundResult>(result);
        }



        [Test]
        public async Task Upsert_WithValidModelStateAndPositiveMonto_CreatesNewProductoAndRedirectsToIndex()
        {
            // Arrange
            var producto = new Producto { Monto = 10 }; // Crear un nuevo producto con un monto positivo
            var productoVM = new ProductoVM { Producto = producto };

            // Configurar el comportamiento esperado en los mocks
            _unidadTrabajoMock.Setup(u => u.Producto.Agregar(It.IsAny<Producto>())).Returns(Task.CompletedTask);
            _unidadTrabajoMock.Setup(u => u.Guardar()).Returns(Task.CompletedTask);

            // Simular una solicitud HTTP con un cuerpo de formulario válido
            var formData = new FormCollection(new Dictionary<string, StringValues>(), new FormFileCollection());
            var httpContext = new DefaultHttpContext { Request = { Form = formData } };
            _controller.ControllerContext = new ControllerContext { HttpContext = httpContext };

            // Act
            var result = await _controller.Upsert(productoVM);

            // Assert
            Assert.IsInstanceOf<RedirectToActionResult>(result);
            var redirectToActionResult = result as RedirectToActionResult;
            Assert.AreEqual("Index", redirectToActionResult.ActionName);
            _unidadTrabajoMock.Verify(u => u.Producto.Agregar(It.IsAny<Producto>()), Times.Once);
            _unidadTrabajoMock.Verify(u => u.Guardar(), Times.Once);
        }






        [Test]
        public async Task Upsert_WithInvalidModelStateAndPositiveMonto_ReturnsViewResultWithProductoVM()
        {
            // Arrange
            var productoVM = new ProductoVM { Producto = new Producto { Monto = 0 } };
            _controller.ModelState.AddModelError("Monto", "Monto debe ser positivo");

            // Act
            var result = await _controller.Upsert(productoVM);

            // Assert
            Assert.IsInstanceOf<ViewResult>(result);
            var viewResult = result as ViewResult;
            Assert.IsInstanceOf<ProductoVM>(viewResult.Model);
            _unidadTrabajoMock.Verify(u => u.Producto.Agregar(It.IsAny<Producto>()), Times.Never);
            _unidadTrabajoMock.Verify(u => u.Guardar(), Times.Never);
        }

    }
}
