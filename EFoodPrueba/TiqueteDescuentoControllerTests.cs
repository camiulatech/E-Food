using System.Security.Claims;
using System.Security.Principal;
using E_Food.Areas.Admin.Controllers;
using EFood.AccesoDatos.Repositorio.IRepositorio;
using EFood.Modelos;
using EFood.Utilidades;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Moq;

namespace E_Food.Tests
{
    [TestFixture]
    public class TiqueteDescuentoControllerTests
    {
        private Mock<IUnidadTrabajo> _unidadTrabajoMock;
        private Mock<IUserStore<IdentityUser>> _userStoreMock;
        private TiqueteDescuentoController _controller;
        private Mock<IUserClaimsPrincipalFactory<IdentityUser>> _userClaimsPrincipalFactoryMock;
        private Mock<ITempDataDictionary> _tempDataMock;

        [SetUp]
        public void Setup()
        {
            _unidadTrabajoMock = new Mock<IUnidadTrabajo>();
            _userStoreMock = new Mock<IUserStore<IdentityUser>>();
            _userClaimsPrincipalFactoryMock = new Mock<IUserClaimsPrincipalFactory<IdentityUser>>();
            _tempDataMock = new Mock<ITempDataDictionary>();

            _controller = new TiqueteDescuentoController(_unidadTrabajoMock.Object)
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
        public async Task Upsert_Post_Crea_Nuevo_TiqueteDescuento_Redirecciona_A_Index()
        {
            
            _unidadTrabajoMock.Setup(u => u.TiqueteDescuento.Agregar(It.IsAny<TiqueteDescuento>())).Returns(Task.CompletedTask);
            _unidadTrabajoMock.Setup(u => u.Guardar()).Returns(Task.CompletedTask);
            _unidadTrabajoMock.Setup(u => u.Bitacora.RegistrarBitacora(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Returns(Task.CompletedTask);

            var nuevoTiqueteDescuento = new TiqueteDescuento { Id = 0, Descripcion = "Nuevo Tiquete", Disponibles = 10, Descuento = 20 };

            
            var result = await _controller.Upsert(nuevoTiqueteDescuento);

            
            Assert.IsInstanceOf<RedirectToActionResult>(result);
            var redirectResult = result as RedirectToActionResult;
            Assert.NotNull(redirectResult);
            Assert.AreEqual("Index", redirectResult.ActionName);
        }

        [Test]
        public async Task Upsert_Post_Crea_Nuevo_TiqueteDescuento_Llama_Agregar_Y_Guardar()
        {
            
            _unidadTrabajoMock.Setup(u => u.TiqueteDescuento.Agregar(It.IsAny<TiqueteDescuento>())).Returns(Task.CompletedTask);
            _unidadTrabajoMock.Setup(u => u.Guardar()).Returns(Task.CompletedTask);
            _unidadTrabajoMock.Setup(u => u.Bitacora.RegistrarBitacora(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Returns(Task.CompletedTask);

            var nuevoTiqueteDescuento = new TiqueteDescuento { Id = 0, Descripcion = "Nuevo Tiquete", Disponibles = 10, Descuento = 20 };

            
            await _controller.Upsert(nuevoTiqueteDescuento);

            
            _unidadTrabajoMock.Verify(u => u.TiqueteDescuento.Agregar(It.IsAny<TiqueteDescuento>()), Times.Once);
            _unidadTrabajoMock.Verify(u => u.Guardar(), Times.Exactly(2));
        }

        [Test]
        public async Task Upsert_Post_Crea_Nuevo_TiqueteDescuento_Registra_Bitacora()
        {
            
            _unidadTrabajoMock.Setup(u => u.TiqueteDescuento.Agregar(It.IsAny<TiqueteDescuento>())).Returns(Task.CompletedTask);
            _unidadTrabajoMock.Setup(u => u.Guardar()).Returns(Task.CompletedTask);
            _unidadTrabajoMock.Setup(u => u.Bitacora.RegistrarBitacora(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Returns(Task.CompletedTask);

            var nuevoTiqueteDescuento = new TiqueteDescuento { Id = 0, Descripcion = "Nuevo Tiquete", Disponibles = 10, Descuento = 20 };

            
            await _controller.Upsert(nuevoTiqueteDescuento);

            
            _unidadTrabajoMock.Verify(u => u.Bitacora.RegistrarBitacora("testuser", "0", "Se insertó el tiquete de descuento 'Nuevo Tiquete' con ID: 0"), Times.Once);
        }

        [Test]
        public async Task Upsert_Post_Crea_Nuevo_TiqueteDescuento_Establece_TempData_Exitosa()
        {
            
            _unidadTrabajoMock.Setup(u => u.TiqueteDescuento.Agregar(It.IsAny<TiqueteDescuento>())).Returns(Task.CompletedTask);
            _unidadTrabajoMock.Setup(u => u.Guardar()).Returns(Task.CompletedTask);
            _unidadTrabajoMock.Setup(u => u.Bitacora.RegistrarBitacora(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Returns(Task.CompletedTask);

            var nuevoTiqueteDescuento = new TiqueteDescuento { Id = 0, Descripcion = "Nuevo Tiquete", Disponibles = 10, Descuento = 20 };

            
            await _controller.Upsert(nuevoTiqueteDescuento);

            
            _tempDataMock.VerifySet(t => t[DS.Exitosa] = "Tiquete de Descuento creado exitosamente", Times.Once);
        }

        [Test]
        public async Task Eliminar_Post_Con_Id_Valido_Retorna_ResultadoJson()
        {
            
            int validId = 1;
            var existingTiqueteDescuento = new TiqueteDescuento { Id = validId, Descripcion = "Tiquete Existente" };
            _unidadTrabajoMock.Setup(u => u.TiqueteDescuento.Obtener(validId)).ReturnsAsync(existingTiqueteDescuento);
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
            var existingTiqueteDescuento = new TiqueteDescuento { Id = validId, Descripcion = "Tiquete Existente" };
            _unidadTrabajoMock.Setup(u => u.TiqueteDescuento.Obtener(validId)).ReturnsAsync(existingTiqueteDescuento);
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

            
            _unidadTrabajoMock.Verify(u => u.TiqueteDescuento.Remover(existingTiqueteDescuento), Times.Once);
            _unidadTrabajoMock.Verify(u => u.Guardar(), Times.Once);
            _unidadTrabajoMock.Verify(u => u.Bitacora.RegistrarBitacora(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Once);
        }

    }
}
