using E_Food.Areas.Admin.Controllers;
using EFood.AccesoDatos.Repositorio.IRepositorio;
using EFood.Modelos;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Linq.Expressions;

namespace E_Food.Tests
{
    [TestFixture]
    public class ErrorControllerTests
    {
        private Mock<IUnidadTrabajo> _unidadTrabajoMock;
        private ErrorController _controller;

        [SetUp]
        public void Setup()
        {
            _unidadTrabajoMock = new Mock<IUnidadTrabajo>();

            _controller = new ErrorController(_unidadTrabajoMock.Object);
        }

        [TearDown]
        public void TearDown()
        {
            _controller.Dispose();
        }

        [Test]
        public void Index_RetornaVista_Vista()
        {
            var result = _controller.Index();

            Assert.IsInstanceOf<ViewResult>(result);
        }

        [Test]
        public async Task ObtenerTodos_DeberiaRetornarJsonConTodosLosRegistrosDeError()
        {
            var registrosMock = new List<Error>(); 
            _unidadTrabajoMock.Setup(u => u.Error.ObtenerTodos(It.IsAny<Expression<Func<Error, bool>>>(), It.IsAny<Func<IQueryable<Error>, IOrderedQueryable<Error>>>(), It.IsAny<string>(), It.IsAny<bool>())).ReturnsAsync(registrosMock);

            var result = await _controller.ObtenerTodos() as JsonResult;

            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Value);
        }

        [Test]
        public async Task ObtenerPorFecha_DeberiaRetornarJsonConLosRegistrosDeErrorDeUnaFechaEspecifica()
        {
            var fecha = new DateTime(2024, 5, 29); 
            var registrosMock = new List<Error>(); 
            _unidadTrabajoMock.Setup(u => u.Error.ObtenerPorFecha(fecha)).ReturnsAsync(registrosMock);

            var result = await _controller.ObtenerPorFecha(fecha) as JsonResult;

            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Value);
        }
    }
}
