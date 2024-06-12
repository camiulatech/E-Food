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
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using EFood.Utilidades;
using System.Security.Principal;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Primitives;
using System.Linq.Expressions;

namespace E_Food.Tests
{
    [TestFixture]
    public class ProductoControllerTests
    {
        private ProductoController _controller;
        private Mock<IUnidadTrabajo> _unidadTrabajoMock;
        private Mock<IWebHostEnvironment> _webHostEnvironmentMock;
        private Mock<ITempDataDictionary> _tempDataMock;

        [SetUp]
        public void SetUp()
        {
            _unidadTrabajoMock = new Mock<IUnidadTrabajo>();
            _webHostEnvironmentMock = new Mock<IWebHostEnvironment>();
            _tempDataMock = new Mock<ITempDataDictionary>();

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
                        }, "mock"))
                    }
                },
                TempData = _tempDataMock.Object
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
        public async Task Eliminar_Post_With_Valid_Id_Calls_Remover_And_Guardar_And_Deletes_Image()
        {
            // Arrange
            int validId = 1;
            var existingProducto = new Producto { Id = validId, Nombre = "Linea Existente", UbicacionImagen = "imagen.jpg" };
            _unidadTrabajoMock.Setup(u => u.Producto.Obtener(validId)).ReturnsAsync(existingProducto);
            _unidadTrabajoMock.Setup(u => u.Guardar()).Returns(Task.CompletedTask);
            _unidadTrabajoMock.Setup(u => u.Bitacora.RegistrarBitacora(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Returns(Task.CompletedTask);

            // Simular la identidad del usuario
            var identity = new GenericIdentity("testuser");
            var principal = new ClaimsPrincipal(identity);

            // Configurar HttpContext
            var httpContext = new DefaultHttpContext
            {
                User = principal
            };

            // Configurar ControllerContext
            var controllerContext = new ControllerContext
            {
                HttpContext = httpContext
            };

            _controller.ControllerContext = controllerContext;

            // Mockear el web host environment
            _webHostEnvironmentMock.Setup(w => w.WebRootPath).Returns("wwwroot");

            // Act
            await _controller.Eliminar(validId);

            // Assert
            _unidadTrabajoMock.Verify(u => u.Producto.Remover(existingProducto), Times.Once);
            _unidadTrabajoMock.Verify(u => u.Guardar(), Times.Once);
            _unidadTrabajoMock.Verify(u => u.Bitacora.RegistrarBitacora(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Once);

            // Verificar que se elimine la imagen
            var expectedImagePath = Path.Combine("wwwroot", "imagen.jpg");
            Assert.IsFalse(File.Exists(expectedImagePath));
        }


        [Test]
        public async Task Eliminar_Post_With_Valid_Id_Returns_JsonResult()
        {
            // Arrange
            int validId = 1;
            var existingProducto = new Producto { Id = validId, Nombre = "Producto Existente", UbicacionImagen = "imagen.jpg" };
            _unidadTrabajoMock.Setup(u => u.Producto.Obtener(validId)).ReturnsAsync(existingProducto);
            _unidadTrabajoMock.Setup(u => u.Bitacora.RegistrarBitacora(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Returns(Task.CompletedTask);

            // Simular la identidad del usuario
            var identity = new GenericIdentity("testuser");
            var principal = new ClaimsPrincipal(identity);

            // Configurar HttpContext
            var httpContext = new DefaultHttpContext
            {
                User = principal
            };

            // Configurar ControllerContext
            var controllerContext = new ControllerContext
            {
                HttpContext = httpContext
            };

            _controller.ControllerContext = controllerContext;

            // Mockear el web host environment
            _webHostEnvironmentMock.Setup(w => w.WebRootPath).Returns("wwwroot");

            // Act
            var result = await _controller.Eliminar(validId);

            // Assert
            Assert.IsInstanceOf<JsonResult>(result);
            var jsonResult = result as JsonResult;
            Assert.IsTrue((bool)jsonResult.Value.GetType().GetProperty("success").GetValue(jsonResult.Value));
        }
        


    }
}


