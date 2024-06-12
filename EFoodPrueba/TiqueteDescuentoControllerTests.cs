using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;
using E_Food.Areas.Admin.Controllers;
using EFood.AccesoDatos.Repositorio.IRepositorio;
using EFood.Modelos;
using EFood.Utilidades;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Moq;
using NUnit.Framework;

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
            // Arrange
            _unidadTrabajoMock.Setup(u => u.TiqueteDescuento.Agregar(It.IsAny<TiqueteDescuento>())).Returns(Task.CompletedTask);
            _unidadTrabajoMock.Setup(u => u.Guardar()).Returns(Task.CompletedTask);
            _unidadTrabajoMock.Setup(u => u.Bitacora.RegistrarBitacora(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Returns(Task.CompletedTask);

            var nuevoTiqueteDescuento = new TiqueteDescuento { Id = 0, Descripcion = "Nuevo Tiquete", Disponibles = 10, Descuento = 20 };

            // Act
            var result = await _controller.Upsert(nuevoTiqueteDescuento);

            // Assert
            Assert.IsInstanceOf<RedirectToActionResult>(result);
            var redirectResult = result as RedirectToActionResult;
            Assert.NotNull(redirectResult);
            Assert.AreEqual("Index", redirectResult.ActionName);
        }

        [Test]
        public async Task Upsert_Post_Crea_Nuevo_TiqueteDescuento_Llama_Agregar_Y_Guardar()
        {
            // Arrange
            _unidadTrabajoMock.Setup(u => u.TiqueteDescuento.Agregar(It.IsAny<TiqueteDescuento>())).Returns(Task.CompletedTask);
            _unidadTrabajoMock.Setup(u => u.Guardar()).Returns(Task.CompletedTask);
            _unidadTrabajoMock.Setup(u => u.Bitacora.RegistrarBitacora(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Returns(Task.CompletedTask);

            var nuevoTiqueteDescuento = new TiqueteDescuento { Id = 0, Descripcion = "Nuevo Tiquete", Disponibles = 10, Descuento = 20 };

            // Act
            await _controller.Upsert(nuevoTiqueteDescuento);

            // Assert
            _unidadTrabajoMock.Verify(u => u.TiqueteDescuento.Agregar(It.IsAny<TiqueteDescuento>()), Times.Once);
            _unidadTrabajoMock.Verify(u => u.Guardar(), Times.Exactly(2));
        }

        [Test]
        public async Task Upsert_Post_Crea_Nuevo_TiqueteDescuento_Registra_Bitacora()
        {
            // Arrange
            _unidadTrabajoMock.Setup(u => u.TiqueteDescuento.Agregar(It.IsAny<TiqueteDescuento>())).Returns(Task.CompletedTask);
            _unidadTrabajoMock.Setup(u => u.Guardar()).Returns(Task.CompletedTask);
            _unidadTrabajoMock.Setup(u => u.Bitacora.RegistrarBitacora(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Returns(Task.CompletedTask);

            var nuevoTiqueteDescuento = new TiqueteDescuento { Id = 0, Descripcion = "Nuevo Tiquete", Disponibles = 10, Descuento = 20 };

            // Act
            await _controller.Upsert(nuevoTiqueteDescuento);

            // Assert
            _unidadTrabajoMock.Verify(u => u.Bitacora.RegistrarBitacora("testuser", "0", "Se insertó el tiquete de descuento 'Nuevo Tiquete' con ID: 0"), Times.Once);
        }

        [Test]
        public async Task Upsert_Post_Crea_Nuevo_TiqueteDescuento_Establece_TempData_Exitosa()
        {
            // Arrange
            _unidadTrabajoMock.Setup(u => u.TiqueteDescuento.Agregar(It.IsAny<TiqueteDescuento>())).Returns(Task.CompletedTask);
            _unidadTrabajoMock.Setup(u => u.Guardar()).Returns(Task.CompletedTask);
            _unidadTrabajoMock.Setup(u => u.Bitacora.RegistrarBitacora(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Returns(Task.CompletedTask);

            var nuevoTiqueteDescuento = new TiqueteDescuento { Id = 0, Descripcion = "Nuevo Tiquete", Disponibles = 10, Descuento = 20 };

            // Act
            await _controller.Upsert(nuevoTiqueteDescuento);

            // Assert
            _tempDataMock.VerifySet(t => t[DS.Exitosa] = "Tiquete de Descuento creado exitosamente", Times.Once);
        }

        [Test]
        public async Task Eliminar_Post_With_Valid_Id_Returns_JsonResult()
        {
            // Arrange
            int validId = 1;
            var existingTiqueteDescuento = new TiqueteDescuento { Id = validId, Descripcion = "Tiquete Existente" };
            _unidadTrabajoMock.Setup(u => u.TiqueteDescuento.Obtener(validId)).ReturnsAsync(existingTiqueteDescuento);
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
            var existingTiqueteDescuento = new TiqueteDescuento { Id = validId, Descripcion = "Tiquete Existente" };
            _unidadTrabajoMock.Setup(u => u.TiqueteDescuento.Obtener(validId)).ReturnsAsync(existingTiqueteDescuento);
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

            // Act
            await _controller.Eliminar(validId);

            // Assert
            _unidadTrabajoMock.Verify(u => u.TiqueteDescuento.Remover(existingTiqueteDescuento), Times.Once);
            _unidadTrabajoMock.Verify(u => u.Guardar(), Times.Once);
            _unidadTrabajoMock.Verify(u => u.Bitacora.RegistrarBitacora(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Once);
        }

        //[Test]
        //public async Task Upsert_Post_Actualiza_TiqueteDescuento_Existente_Redirecciona_A_Index()
        //{
        //    // Arrange
        //    _unidadTrabajoMock.Setup(u => u.TiqueteDescuento.Actualizar(It.IsAny<TiqueteDescuento>()));
        //    _unidadTrabajoMock.Setup(u => u.Guardar()).Returns(Task.CompletedTask);
        //    _unidadTrabajoMock.Setup(u => u.Bitacora.RegistrarBitacora(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Returns(Task.CompletedTask);

        //    var existingTiqueteDescuento = new TiqueteDescuento { Id = 1, Descripcion = "Tiquete Existente" };

        //    // Act
        //    var result = await _controller.Upsert(existingTiqueteDescuento);

        //    // Assert
        //    Assert.IsInstanceOf<RedirectToActionResult>(result);
        //    var redirectResult = result as RedirectToActionResult;
        //    Assert.NotNull(redirectResult);
        //    Assert.AreEqual("Index", redirectResult.ActionName);
        //}

        //[Test]
        //public async Task Upsert_Post_Actualiza_TiqueteDescuento_Existente_Llama_Actualizar_Y_Guardar()
        //{
        //    // Arrange
        //    _unidadTrabajoMock.Setup(u => u.TiqueteDescuento.Actualizar(It.IsAny<TiqueteDescuento>()));
        //    _unidadTrabajoMock.Setup(u => u.Guardar()).Returns(Task.CompletedTask);
        //    _unidadTrabajoMock.Setup(u => u.Bitacora.RegistrarBitacora(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Returns(Task.CompletedTask);

        //    var existingTiqueteDescuento = new TiqueteDescuento { Id = 1, Descripcion = "Tiquete Existente" };

        //    // Act
        //    await _controller.Upsert(existingTiqueteDescuento);

        //    // Assert
        //    _unidadTrabajoMock.Verify(u => u.TiqueteDescuento.Actualizar(It.IsAny<TiqueteDescuento>()), Times.Once);
        //    _unidadTrabajoMock.Verify(u => u.Guardar(), Times.Once);
        //}

        //[Test]
        //public async Task Upsert_Post_Actualiza_TiqueteDescuento_Existente_Registra_Bitacora()
        //{
        //    // Arrange
        //    _unidadTrabajoMock.Setup(u => u.TiqueteDescuento.Actualizar(It.IsAny<TiqueteDescuento>()));
        //    _unidadTrabajoMock.Setup(u => u.Guardar()).Returns(Task.CompletedTask);
        //    _unidadTrabajoMock.Setup(u => u.Bitacora.RegistrarBitacora(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Returns(Task.CompletedTask);

        //    var existingTiqueteDescuento = new TiqueteDescuento { Id = 1, Descripcion = "Tiquete Existente" };

        //    // Act
        //    await _controller.Upsert(existingTiqueteDescuento);

        //    // Assert
        //    _unidadTrabajoMock.Verify(u => u.Bitacora.RegistrarBitacora("testuser", "1", "Se actualizó el tiquete de descuento 'Tiquete Existente' con ID: 1"), Times.Once);
        //}

        //[Test]
        //public async Task Upsert_Post_Actualiza_TiqueteDescuento_Existente_Establece_TempData_Exitosa()
        //{
        //    // Arrange
        //    _unidadTrabajoMock.Setup(u => u.TiqueteDescuento.Actualizar(It.IsAny<TiqueteDescuento>()));
        //    _unidadTrabajoMock.Setup(u => u.Guardar()).Returns(Task.CompletedTask);
        //    _unidadTrabajoMock.Setup(u => u.Bitacora.RegistrarBitacora(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Returns(Task.CompletedTask);

        //    var existingTiqueteDescuento = new TiqueteDescuento { Id = 1, Descripcion = "Tiquete Existente" };

        //    // Act
        //    await _controller.Upsert(existingTiqueteDescuento);

        //    // Assert
        //    _tempDataMock.VerifySet(t => t[DS.Exitosa] = "Tiquete de Descuento actualizado exitosamente", Times.Once);
        //}


    }
}
