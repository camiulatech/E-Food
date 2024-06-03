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
        public async Task Eliminar_IdIsValid_ReturnsJsonResultWithSuccess()
        {
            // Arrange
            var productoId = 1;
            var producto = new Producto { Id = productoId, Nombre = "TestProducto", UbicacionImagen = "imagen.jpg" };
            _unidadTrabajoMock.Setup(u => u.Producto.Obtener(productoId)).ReturnsAsync(producto);

            // Act
            var result = await _controller.Eliminar(productoId);

            // Assert
            Assert.IsInstanceOf<JsonResult>(result);
            var jsonResult = result as JsonResult;
            dynamic data = jsonResult.Value;
            Assert.IsTrue(data.success);

            // Verificar que se elimine el archivo de imagen
            _unidadTrabajoMock.Verify(u => u.Producto.Remover(It.IsAny<Producto>()), Times.Once);
            _unidadTrabajoMock.Verify(u => u.Guardar(), Times.Once);

            // Verificar que User.Identity no sea nulo
            Assert.NotNull(_controller.User.Identity);
        }


        [Test]
        public async Task Eliminar_IdIsNotValid_ReturnsJsonResultWithError()
        {
            // Arrange
            var productoId = 1;
            Producto producto = null;
            _unidadTrabajoMock.Setup(u => u.Producto.Obtener(productoId)).ReturnsAsync(producto);

            // Act
            var result = await _controller.Eliminar(productoId);

            // Assert
            Assert.IsInstanceOf<JsonResult>(result);
            var jsonResult = result as JsonResult;
            dynamic data = jsonResult.Value;
            Assert.IsFalse(data.success);
        }


    }


}

