using E_Food.Areas.Admin.Controllers;
using EFood.AccesoDatos.Repositorio.IRepositorio;
using EFood.Modelos;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

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
            // Act
            var result = _controller.Index();

            // Assert
            Assert.IsInstanceOf<ViewResult>(result);
        }

        [Test]
        public async Task ObtenerTodos_DeberiaRetornarJsonConTodosLosRegistrosDeError()
        {
            // Arrange
            var registrosMock = new List<Error>(); // Simula una lista de registros de error
            _unidadTrabajoMock.Setup(u => u.Error.ObtenerTodos(It.IsAny<Expression<Func<Error, bool>>>(), It.IsAny<Func<IQueryable<Error>, IOrderedQueryable<Error>>>(), It.IsAny<string>(), It.IsAny<bool>())).ReturnsAsync(registrosMock);

            // Act
            var result = await _controller.ObtenerTodos() as JsonResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Value);
            // Verifica la estructura del JSON retornado según lo esperado
        }

        [Test]
        public async Task ObtenerPorFecha_DeberiaRetornarJsonConLosRegistrosDeErrorDeUnaFechaEspecifica()
        {
            // Arrange
            var fecha = new DateTime(2024, 5, 29); // Puedes establecer una fecha específica para la prueba
            var registrosMock = new List<Error>(); // Simula una lista de registros de error
            _unidadTrabajoMock.Setup(u => u.Error.ObtenerPorFecha(fecha)).ReturnsAsync(registrosMock);

            // Act
            var result = await _controller.ObtenerPorFecha(fecha) as JsonResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Value);
            // Verifica la estructura del JSON retornado según lo esperado
        }
    }
}
