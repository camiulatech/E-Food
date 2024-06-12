﻿using E_Food.Areas.Admin.Controllers;
using EFood.AccesoDatos.Repositorio.IRepositorio;
using EFood.Modelos;
using EFood.Utilidades;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
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

            _controller = new BitacoraController(_unidadTrabajoMock.Object);
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
        public async Task ObtenerTodos_RetornarJsonConLosRegistrosDeBitacora()
        {
            // Arrange
            var registrosMock = new List<Bitacora>(); // Simula una lista de registros de bitácora
            _unidadTrabajoMock.Setup(u => u.Bitacora.ObtenerTodos(It.IsAny<Expression<Func<Bitacora, bool>>>(), It.IsAny<Func<IQueryable<Bitacora>, IOrderedQueryable<Bitacora>>>(), It.IsAny<string>(), It.IsAny<bool>())).ReturnsAsync(registrosMock);

            // Act
            var result = await _controller.ObtenerTodos() as JsonResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Value);
            // Verifica la estructura del JSON retornado según lo esperado
        }


        [Test]
        public async Task ObtenerPorFecha_RetornarJsonConLosRegistrosDeBitacoraDeUnaFechaEspecifica()
        {
            // Arrange
            var fecha = new DateTime(2024, 5, 29); // Puedes establecer una fecha específica para la prueba
            var registrosMock = new List<Bitacora>(); // Simula una lista de registros de bitácora
            _unidadTrabajoMock.Setup(u => u.Bitacora.ObtenerPorFecha(fecha)).ReturnsAsync(registrosMock);

            // Act
            var result = await _controller.ObtenerPorFecha(fecha) as JsonResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Value);
            // Verifica la estructura del JSON retornado según lo esperado
        }


    }
}
