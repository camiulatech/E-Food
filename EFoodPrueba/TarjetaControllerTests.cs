using System.Security.Claims;
using E_Food.Areas.Admin.Controllers;
using EFood.AccesoDatos.Repositorio.IRepositorio;
using EFood.Modelos;
using EFood.Utilidades;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System.Security.Principal;

namespace E_Food.Tests
{
    [TestFixture]
    public class TarjetaControllerTests
    {
        private Mock<IUnidadTrabajo> _unidadTrabajoMock;
        private TarjetaController _controller;
        private Mock<ITempDataDictionary> _tempDataMock;

        [SetUp]
        public void Setup()
        {
            _unidadTrabajoMock = new Mock<IUnidadTrabajo>();
            _tempDataMock = new Mock<ITempDataDictionary>();

            _controller = new TarjetaController(_unidadTrabajoMock.Object)
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
        public async Task Upsert_Post_Crea_Nueva_Tarjeta_Redirecciona_A_Index()
        {
            
            _unidadTrabajoMock.Setup(u => u.Tarjeta.Agregar(It.IsAny<Tarjeta>())).Returns(Task.CompletedTask);
            _unidadTrabajoMock.Setup(u => u.Guardar()).Returns(Task.CompletedTask);
            _unidadTrabajoMock.Setup(u => u.Bitacora.RegistrarBitacora(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Returns(Task.CompletedTask);

            var nuevaTarjeta = new Tarjeta { Id = 0, Nombre = "Nueva Tarjeta" };

            
            var result = await _controller.Upsert(nuevaTarjeta);

            
            Assert.IsInstanceOf<RedirectToActionResult>(result);
            var redirectResult = result as RedirectToActionResult;
            Assert.NotNull(redirectResult);
            Assert.AreEqual("Index", redirectResult.ActionName);
        }

        [Test]
        public async Task Upsert_Post_Crea_Nueva_Tarjeta_Llama_Agregar_Y_Guardar()
        {
            
            _unidadTrabajoMock.Setup(u => u.Tarjeta.Agregar(It.IsAny<Tarjeta>())).Returns(Task.CompletedTask);
            _unidadTrabajoMock.Setup(u => u.Guardar()).Returns(Task.CompletedTask);
            _unidadTrabajoMock.Setup(u => u.Bitacora.RegistrarBitacora(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Returns(Task.CompletedTask);

            var nuevaTarjeta = new Tarjeta { Id = 0, Nombre = "Nueva Tarjeta" };

            
            await _controller.Upsert(nuevaTarjeta);

            
            _unidadTrabajoMock.Verify(u => u.Tarjeta.Agregar(It.IsAny<Tarjeta>()), Times.Once);
            _unidadTrabajoMock.Verify(u => u.Guardar(), Times.Exactly(2));
        }

        [Test]
        public async Task Upsert_Post_Crea_Nueva_Tarjeta_Registra_Bitacora()
        {
            
            _unidadTrabajoMock.Setup(u => u.Tarjeta.Agregar(It.IsAny<Tarjeta>())).Returns(Task.CompletedTask);
            _unidadTrabajoMock.Setup(u => u.Guardar()).Returns(Task.CompletedTask);
            _unidadTrabajoMock.Setup(u => u.Bitacora.RegistrarBitacora(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Returns(Task.CompletedTask);

            var nuevaTarjeta = new Tarjeta { Id = 0, Nombre = "Nueva Tarjeta" };

            
            await _controller.Upsert(nuevaTarjeta);

            
            _unidadTrabajoMock.Verify(u => u.Bitacora.RegistrarBitacora("testuser", "0", "Se insertó la tarjeta 'Nueva Tarjeta' con ID: 0"), Times.Once);
        }

        [Test]
        public async Task Upsert_Post_Crea_Nueva_Tarjeta_Establece_TempData_Exitosa()
        {
            
            _unidadTrabajoMock.Setup(u => u.Tarjeta.Agregar(It.IsAny<Tarjeta>())).Returns(Task.CompletedTask);
            _unidadTrabajoMock.Setup(u => u.Guardar()).Returns(Task.CompletedTask);
            _unidadTrabajoMock.Setup(u => u.Bitacora.RegistrarBitacora(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Returns(Task.CompletedTask);

            var nuevaTarjeta = new Tarjeta { Id = 0, Nombre = "Nueva Tarjeta" };

            
            await _controller.Upsert(nuevaTarjeta);

            
            _tempDataMock.VerifySet(t => t[DS.Exitosa] = "Tarjeta creada exitosamente", Times.Once);
        }

        [Test]
        public async Task Upsert_Post_Actualiza_Tarjeta_Existente_Redirecciona_A_Index()
        {
            
            _unidadTrabajoMock.Setup(u => u.Tarjeta.Actualizar(It.IsAny<Tarjeta>()));
            _unidadTrabajoMock.Setup(u => u.Guardar()).Returns(Task.CompletedTask);
            _unidadTrabajoMock.Setup(u => u.Bitacora.RegistrarBitacora(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Returns(Task.CompletedTask);

            var existingTarjeta = new Tarjeta { Id = 1, Nombre = "Tarjeta Existente" };

            
            var result = await _controller.Upsert(existingTarjeta);

            
            Assert.IsInstanceOf<RedirectToActionResult>(result);
            var redirectResult = result as RedirectToActionResult;
            Assert.NotNull(redirectResult);
            Assert.AreEqual("Index", redirectResult.ActionName);
        }

        [Test]
        public async Task Upsert_Post_Actualiza_Tarjeta_Existente_Llama_Actualizar_Y_Guardar()
        {
            
            _unidadTrabajoMock.Setup(u => u.Tarjeta.Actualizar(It.IsAny<Tarjeta>()));
            _unidadTrabajoMock.Setup(u => u.Guardar()).Returns(Task.CompletedTask);
            _unidadTrabajoMock.Setup(u => u.Bitacora.RegistrarBitacora(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Returns(Task.CompletedTask);

            var existingTarjeta = new Tarjeta { Id = 1, Nombre = "Tarjeta Existente" };

            
            await _controller.Upsert(existingTarjeta);

            
            _unidadTrabajoMock.Verify(u => u.Tarjeta.Actualizar(It.IsAny<Tarjeta>()), Times.Once);
            _unidadTrabajoMock.Verify(u => u.Guardar(), Times.Once);
        }

        [Test]
        public async Task Upsert_Post_Actualiza_Tarjeta_Existente_Registra_Bitacora()
        {
            
            _unidadTrabajoMock.Setup(u => u.Tarjeta.Actualizar(It.IsAny<Tarjeta>()));
            _unidadTrabajoMock.Setup(u => u.Guardar()).Returns(Task.CompletedTask);
            _unidadTrabajoMock.Setup(u => u.Bitacora.RegistrarBitacora(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Returns(Task.CompletedTask);

            var existingTarjeta = new Tarjeta { Id = 1, Nombre = "Tarjeta Existente" };

            
            await _controller.Upsert(existingTarjeta);

            
            _unidadTrabajoMock.Verify(u => u.Bitacora.RegistrarBitacora("testuser", "1", "Se actualizó la tarjeta 'Tarjeta Existente' con ID: 1"), Times.Once);
        }

        [Test]
        public async Task Upsert_Post_Actualiza_Tarjeta_Existente_Establece_TempData_Exitosa()
        {
            
            _unidadTrabajoMock.Setup(u => u.Tarjeta.Actualizar(It.IsAny<Tarjeta>()));
            _unidadTrabajoMock.Setup(u => u.Guardar()).Returns(Task.CompletedTask);
            _unidadTrabajoMock.Setup(u => u.Bitacora.RegistrarBitacora(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Returns(Task.CompletedTask);

            var existingTarjeta = new Tarjeta { Id = 1, Nombre = "Tarjeta Existente" };

            
            await _controller.Upsert(existingTarjeta);

            
            _tempDataMock.VerifySet(t => t[DS.Exitosa] = "Tarjeta actualizada exitosamente", Times.Once);
        }

        [Test]
        public async Task Eliminar_Post_Con_Id_Valido_Returna_ResultadoJson()
        {
            
            int validId = 1;
            var existingTarjeta = new Tarjeta { Id = validId, Nombre = "Tarjeta Existente" };
            _unidadTrabajoMock.Setup(u => u.Tarjeta.Obtener(validId)).ReturnsAsync(existingTarjeta);
            _unidadTrabajoMock.Setup(u => u.Bitacora.RegistrarBitacora(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Returns(Task.CompletedTask);

            var identity = new GenericIdentity("testuser");
            var principal = new ClaimsPrincipal(identity);

            var httpContext = new DefaultHttpContext
            {
                User = principal
            };

            var controllerContext = new ControllerContext
            {
                HttpContext = httpContext
            };

            _controller.ControllerContext = controllerContext;

            
            var result = await _controller.Eliminar(validId);

            
            Assert.IsInstanceOf<JsonResult>(result);
        }

        [Test]
        public async Task Eliminar_Post_Con_Id_Valido_Llama_Remover_Y_Guardar()
        {
            
            int validId = 1;
            var existingTarjeta = new Tarjeta { Id = validId, Nombre = "Tarjeta Existente" };
            _unidadTrabajoMock.Setup(u => u.Tarjeta.Obtener(validId)).ReturnsAsync(existingTarjeta);
            _unidadTrabajoMock.Setup(u => u.Guardar()).Returns(Task.CompletedTask);
            _unidadTrabajoMock.Setup(u => u.Bitacora.RegistrarBitacora(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Returns(Task.CompletedTask);

            var identity = new GenericIdentity("testuser");
            var principal = new ClaimsPrincipal(identity);

            var httpContext = new DefaultHttpContext
            {
                User = principal
            };

            var controllerContext = new ControllerContext
            {
                HttpContext = httpContext
            };

            _controller.ControllerContext = controllerContext;

            
            await _controller.Eliminar(validId);

            
            _unidadTrabajoMock.Verify(u => u.Tarjeta.Remover(existingTarjeta), Times.Once);
            _unidadTrabajoMock.Verify(u => u.Guardar(), Times.Once);
            _unidadTrabajoMock.Verify(u => u.Bitacora.RegistrarBitacora(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Once);
        }
    }
}
