using E_Food.Areas.Admin.Controllers;
using EFood.AccesoDatos.Repositorio.IRepositorio;
using EFood.Modelos;
using EFood.Utilidades;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace E_Food.Tests
{
    [TestFixture]
    public class BitacoraControllerTests
    {
        private Mock<IUnidadTrabajo> _unidadTrabajoMock;
        private BitacoraController _controller;

        [SetUp]
        public void Setup()
        {
            _unidadTrabajoMock = new Mock<IUnidadTrabajo>();

            _controller = new BitacoraController(_unidadTrabajoMock.Object)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = new DefaultHttpContext
                    {
                        User = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
                        {
                            new Claim(ClaimTypes.Name, "testuser")
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
        public void Index_Returns_ViewResult()
        {
            // Act
            var result = _controller.Index();

            // Assert
            Assert.IsInstanceOf<ViewResult>(result);
        }

        [Test]
        public async Task ObtenerTodos_Returns_JsonResult_With_Data()
        {
            // Arrange
            var bitacoraList = new List<Bitacora>
            {
                new Bitacora { Id = 1, Usuario = "testuser", Fecha = DateTime.Now , CodigoRegistro = "1", Descripcion = "Accion 1"},
                new Bitacora { Id = 2, Usuario = "testuser", Fecha = DateTime.Now , CodigoRegistro = "2", Descripcion = "Accion 2"}
            };

            _unidadTrabajoMock.Setup(u => u.Bitacora.ObtenerTodos()).ReturnsAsync(bitacoraList);

            // Act
            var result = await _controller.ObtenerTodos();

            // Assert
            var jsonResult = result as JsonResult;
            Assert.NotNull(jsonResult);
            dynamic data = jsonResult.Value;
            Assert.AreEqual(bitacoraList, data.data);
        }

        [Test]
        public async Task ObtenerPorFecha_Returns_JsonResult_With_Data()
        {
            // Arrange
            var fecha = DateTime.Now;
            var bitacoraList = new List<Bitacora>
            {
                new Bitacora { Id = 1, UsuarioNombre = "testuser", Accion = "Accion 1", Fecha = fecha },
                new Bitacora { Id = 2, UsuarioNombre = "testuser", Accion = "Accion 2", Fecha = fecha }
            };

            _unidadTrabajoMock.Setup(u => u.Bitacora.ObtenerPorFecha(fecha)).ReturnsAsync(bitacoraList);

            // Act
            var result = await _controller.ObtenerPorFecha(fecha);

            // Assert
            var jsonResult = result as JsonResult;
            Assert.NotNull(jsonResult);
            dynamic data = jsonResult.Value;
            Assert.AreEqual(bitacoraList, data.data);
        }
    }
}
