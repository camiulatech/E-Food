using NUnit.Framework;
using Moq;
using E_Food.Areas.Admin.Controllers;
using EFood.AccesoDatos.Repositorio;
using EFood.Modelos;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Security.Claims;
using System.Collections.Generic;
using EFood.AccesoDatos.Repositorio.IRepositorio;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System.Security.Principal;

namespace EFoodTests
{
    [TestFixture]
    public class LineaComidaControllerTests
    {
        private LineaComidaController _controller;
        private Mock<IUnidadTrabajo> _unidadTrabajoMock;

        [SetUp]
        public void Setup()
        {
            _unidadTrabajoMock = new Mock<IUnidadTrabajo>();
            _controller = new LineaComidaController(_unidadTrabajoMock.Object);
            var httpContext = new DefaultHttpContext();
            httpContext.User = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Name, "usuarioEjemplo")
            }, "mock"));

            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = httpContext
            };
        }


        [TearDown]
        public void TearDown()
        {
            _controller.Dispose();
        }

        [Test]
        public async Task Test_Index_Returns_ViewResult()
        {
            // Act
            var result = _controller.Index().ExecuteResultAsync;

            // Assert
            Assert.IsInstanceOf<ViewResult>(result);
        }

        [Test]
        public async Task Test_Upsert_With_Null_Id_Returns_ViewResult()
        {
            // Act
            var result = await _controller.Upsert((int?)null);

            // Assert
            Assert.IsInstanceOf<ViewResult>(result);
        }

        [Test]
        public async Task Test_Upsert_With_Valid_Id_Returns_ViewResult()
        {
            // Arrange
            int validId = 1;
            _unidadTrabajoMock.Setup(u => u.LineaComida.Obtener(validId)).ReturnsAsync(new LineaComida());

            // Act
            var result = await _controller.Upsert(validId);

            // Assert
            Assert.IsInstanceOf<ViewResult>(result);
        }

        [Test]
        public async Task Test_Upsert_Post_With_Valid_Model_Returns_RedirectToActionResult()
        {
            // Arrange
            _unidadTrabajoMock.Setup(u => u.Guardar()).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.Upsert(new LineaComida());

            // Assert
            Assert.IsInstanceOf<RedirectToActionResult>(result);
        }

        [Test]
        public async Task Test_Eliminar_With_Valid_Id_Returns_JsonResult()
        {
            // Arrange
            int validId = 1;
            _unidadTrabajoMock.Setup(u => u.LineaComida.Obtener(validId)).ReturnsAsync(new LineaComida());

            // Simular la identidad del usuario
            var identity = new GenericIdentity("usuarioEjemplo"); // Nombre de usuario de ejemplo
            var principal = new ClaimsPrincipal(identity);

            // Configurar HttpContext
            var httpContext = new DefaultHttpContext();
            httpContext.User = principal;

            // Configurar ControllerContext
            var controllerContext = new ControllerContext
            {
                HttpContext = httpContext
            };

            _controller.ControllerContext = controllerContext;

            // Act
            var result = await _controller.Eliminar(validId);

            // Assert
            Assert.IsInstanceOf<JsonResult>(result);
        }






        // Dummy implementation of IServiceProvider
        private class DummyProvider : IServiceProvider
        {
            public object GetService(System.Type serviceType)
            {
                return null;
            }
        }
    }
}
