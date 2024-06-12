using System.Security.Claims;
using System.Threading.Tasks;
using E_Food.Areas.Admin.Controllers;
using EFood.AccesoDatos.Repositorio.IRepositorio;
using EFood.Modelos;
using EFood.Utilidades;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System.Security.Principal;
using System.Linq;

namespace E_Food.Tests
{
    [TestFixture]
    public class ProcesadorPagoControllerTests
    {
        private Mock<IUnidadTrabajo> _unidadTrabajoMock;
        private ProcesadorPagoController _controller;
        private Mock<ITempDataDictionary> _tempDataMock;

        [SetUp]
        public void Setup()
        {
            _unidadTrabajoMock = new Mock<IUnidadTrabajo>();
            _tempDataMock = new Mock<ITempDataDictionary>();

            _controller = new ProcesadorPagoController(_unidadTrabajoMock.Object)
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
        public async Task Upsert_Post_Crea_Nuevo_ProcesadorPago_Redirecciona_A_Index()
        {
            // Arrange
            _unidadTrabajoMock.Setup(u => u.ProcesadorPago.Agregar(It.IsAny<ProcesadorPago>())).Returns(Task.CompletedTask);
            _unidadTrabajoMock.Setup(u => u.Guardar()).Returns(Task.CompletedTask);
            _unidadTrabajoMock.Setup(u => u.Bitacora.RegistrarBitacora(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Returns(Task.CompletedTask);

            var nuevoProcesador = new ProcesadorPago { Id = 0, Procesador = "Nuevo Procesador", Tipo = TipoProcesadorPago.TarjetaDebitoCredito };

            // Act
            var result = await _controller.Upsert(nuevoProcesador);

            // Assert
            Assert.IsInstanceOf<RedirectToActionResult>(result);
            var redirectResult = result as RedirectToActionResult;
            Assert.NotNull(redirectResult);
            Assert.AreEqual("Index", redirectResult.ActionName);
        }

        [Test]
        public async Task Upsert_Post_Crea_Nuevo_ProcesadorPago_Llama_Agregar_Y_Guardar()
        {
            // Arrange
            _unidadTrabajoMock.Setup(u => u.ProcesadorPago.Agregar(It.IsAny<ProcesadorPago>())).Returns(Task.CompletedTask);
            _unidadTrabajoMock.Setup(u => u.Guardar()).Returns(Task.CompletedTask);
            _unidadTrabajoMock.Setup(u => u.Bitacora.RegistrarBitacora(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Returns(Task.CompletedTask);

            var nuevoProcesador = new ProcesadorPago { Id = 0, Procesador = "Nuevo Procesador", Tipo = TipoProcesadorPago.TarjetaDebitoCredito };

            // Act
            await _controller.Upsert(nuevoProcesador);

            // Assert
            _unidadTrabajoMock.Verify(u => u.ProcesadorPago.Agregar(It.IsAny<ProcesadorPago>()), Times.Once);
            _unidadTrabajoMock.Verify(u => u.Guardar(), Times.Exactly(2));
        }

        [Test]
        public async Task Upsert_Post_Crea_Nuevo_ProcesadorPago_Registra_Bitacora()
        {
            // Arrange
            _unidadTrabajoMock.Setup(u => u.ProcesadorPago.Agregar(It.IsAny<ProcesadorPago>())).Returns(Task.CompletedTask);
            _unidadTrabajoMock.Setup(u => u.Guardar()).Returns(Task.CompletedTask);
            _unidadTrabajoMock.Setup(u => u.Bitacora.RegistrarBitacora(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Returns(Task.CompletedTask);

            var nuevoProcesador = new ProcesadorPago { Id = 0, Procesador = "Nuevo Procesador", Tipo = TipoProcesadorPago.TarjetaDebitoCredito };

            // Act
            await _controller.Upsert(nuevoProcesador);

            // Assert
            _unidadTrabajoMock.Verify(u => u.Bitacora.RegistrarBitacora("testuser", "0", "Se insertó el procesador de pago 'Nuevo Procesador' con ID: 0"), Times.Once);
        }

        [Test]
        public async Task Upsert_Post_Crea_Nuevo_ProcesadorPago_Establece_TempData_Exitosa()
        {
            // Arrange
            _unidadTrabajoMock.Setup(u => u.ProcesadorPago.Agregar(It.IsAny<ProcesadorPago>())).Returns(Task.CompletedTask);
            _unidadTrabajoMock.Setup(u => u.Guardar()).Returns(Task.CompletedTask);
            _unidadTrabajoMock.Setup(u => u.Bitacora.RegistrarBitacora(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Returns(Task.CompletedTask);

            var nuevoProcesador = new ProcesadorPago { Id = 0, Procesador = "Nuevo Procesador", Tipo = TipoProcesadorPago.TarjetaDebitoCredito };

            // Act
            await _controller.Upsert(nuevoProcesador);

            // Assert
            _tempDataMock.VerifySet(t => t[DS.Exitosa] = "Procesador de pago creado exitosamente", Times.Once);
        }

        [Test]
        public async Task Upsert_Post_Actualiza_ProcesadorPago_Existente_Redirecciona_A_Index()
        {
            // Arrange
            _unidadTrabajoMock.Setup(u => u.ProcesadorPago.Actualizar(It.IsAny<ProcesadorPago>()));
            _unidadTrabajoMock.Setup(u => u.Guardar()).Returns(Task.CompletedTask);
            _unidadTrabajoMock.Setup(u => u.Bitacora.RegistrarBitacora(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Returns(Task.CompletedTask);

            var existingProcesador = new ProcesadorPago { Id = 1, Procesador = "Procesador Existente", Tipo = TipoProcesadorPago.TarjetaDebitoCredito };

            // Act
            var result = await _controller.Upsert(existingProcesador);

            // Assert
            Assert.IsInstanceOf<RedirectToActionResult>(result);
            var redirectResult = result as RedirectToActionResult;
            Assert.NotNull(redirectResult);
            Assert.AreEqual("Index", redirectResult.ActionName);
        }

        [Test]
        public async Task Upsert_Post_Actualiza_ProcesadorPago_Existente_Llama_Actualizar_Y_Guardar()
        {
            // Arrange
            _unidadTrabajoMock.Setup(u => u.ProcesadorPago.Actualizar(It.IsAny<ProcesadorPago>()));
            _unidadTrabajoMock.Setup(u => u.Guardar()).Returns(Task.CompletedTask);
            _unidadTrabajoMock.Setup(u => u.Bitacora.RegistrarBitacora(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Returns(Task.CompletedTask);

            var existingProcesador = new ProcesadorPago { Id = 1, Procesador = "Procesador Existente", Tipo = TipoProcesadorPago.TarjetaDebitoCredito };

            // Act
            await _controller.Upsert(existingProcesador);

            // Assert
            _unidadTrabajoMock.Verify(u => u.ProcesadorPago.Actualizar(It.IsAny<ProcesadorPago>()), Times.Once);
            _unidadTrabajoMock.Verify(u => u.Guardar(), Times.Once);
        }

        [Test]
        public async Task Upsert_Post_Actualiza_ProcesadorPago_Existente_Registra_Bitacora()
        {
            // Arrange
            _unidadTrabajoMock.Setup(u => u.ProcesadorPago.Actualizar(It.IsAny<ProcesadorPago>()));
            _unidadTrabajoMock.Setup(u => u.Guardar()).Returns(Task.CompletedTask);
            _unidadTrabajoMock.Setup(u => u.Bitacora.RegistrarBitacora(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Returns(Task.CompletedTask);

            var existingProcesador = new ProcesadorPago { Id = 1, Procesador = "Procesador Existente", Tipo = TipoProcesadorPago.TarjetaDebitoCredito };

            // Act
            await _controller.Upsert(existingProcesador);

            // Assert
            _unidadTrabajoMock.Verify(u => u.Bitacora.RegistrarBitacora("testuser", "1", "Se actualizó el procesador de pago 'Procesador Existente' con ID: 1"), Times.Once);
        }

        [Test]
        public async Task Upsert_Post_Actualiza_ProcesadorPago_Existente_Establece_TempData_Exitosa()
        {
            // Arrange
            _unidadTrabajoMock.Setup(u => u.ProcesadorPago.Actualizar(It.IsAny<ProcesadorPago>()));
            _unidadTrabajoMock.Setup(u => u.Guardar()).Returns(Task.CompletedTask);
            _unidadTrabajoMock.Setup(u => u.Bitacora.RegistrarBitacora(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Returns(Task.CompletedTask);

            var existingProcesador = new ProcesadorPago { Id = 1, Procesador = "Procesador Existente", Tipo = TipoProcesadorPago.TarjetaDebitoCredito };

            // Act
            await _controller.Upsert(existingProcesador);

            // Assert
            _tempDataMock.VerifySet(t => t[DS.Exitosa] = "Procesador de pago actualizado exitosamente", Times.Once);
        }

        [Test]
        public async Task Eliminar_Post_With_Valid_Id_Returns_JsonResult()
        {
            // Arrange
            int validId = 1;
            var existingProcesador = new ProcesadorPago { Id = validId, Procesador = "Procesador Existente" };
            _unidadTrabajoMock.Setup(u => u.ProcesadorPago.Obtener(validId)).ReturnsAsync(existingProcesador);
            _unidadTrabajoMock.Setup(u => u.Guardar()).Returns(Task.CompletedTask);
            _unidadTrabajoMock.Setup(u => u.Bitacora.RegistrarBitacora(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.Eliminar(validId);

            // Assert
            Assert.IsInstanceOf<JsonResult>(result);
        }

        [Test]
        public async Task Eliminar_Post_With_Valid_Id_Calls_Remover_And_Guardar()
        {
            // Arrange
            int validId = 1;
            var existingProcesador = new ProcesadorPago { Id = validId, Procesador = "Procesador Existente" };
            _unidadTrabajoMock.Setup(u => u.ProcesadorPago.Obtener(validId)).ReturnsAsync(existingProcesador);
            _unidadTrabajoMock.Setup(u => u.Guardar()).Returns(Task.CompletedTask);
            _unidadTrabajoMock.Setup(u => u.Bitacora.RegistrarBitacora(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Returns(Task.CompletedTask);

            // Act
            await _controller.Eliminar(validId);

            // Assert
            _unidadTrabajoMock.Verify(u => u.ProcesadorPago.Remover(existingProcesador), Times.Once);
            _unidadTrabajoMock.Verify(u => u.Guardar(), Times.Once);
            _unidadTrabajoMock.Verify(u => u.Bitacora.RegistrarBitacora(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Once);
        }
    }
}
