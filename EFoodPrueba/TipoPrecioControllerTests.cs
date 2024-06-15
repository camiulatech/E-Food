using System.Security.Claims;
using E_Food.Areas.Admin.Controllers;
using EFood.AccesoDatos.Repositorio.IRepositorio;
using EFood.Modelos;
using EFood.Utilidades;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace E_Food.Tests
{
    [TestFixture]
    public class TipoPrecioControllerTests
    {
        private Mock<IUnidadTrabajo> _unidadTrabajoMock;
        private TipoPrecioController _controller;
        private Mock<ITempDataDictionary> _tempDataMock;
        private Mock<IUserStore<IdentityUser>> _userStoreMock;
        private Mock<IUserClaimsPrincipalFactory<IdentityUser>> _userClaimsPrincipalFactoryMock;

        [SetUp]
        public void Setup()
        {
            _unidadTrabajoMock = new Mock<IUnidadTrabajo>();
            _userStoreMock = new Mock<IUserStore<IdentityUser>>();
            _userClaimsPrincipalFactoryMock = new Mock<IUserClaimsPrincipalFactory<IdentityUser>>();
            _tempDataMock = new Mock<ITempDataDictionary>();

            _controller = new TipoPrecioController(_unidadTrabajoMock.Object)
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
        public async Task Upsert_Post_Crea_Nuevo_TipoPrecio_Redirecciona_A_Index()
        {
            
            _unidadTrabajoMock.Setup(u => u.TipoPrecio.Agregar(It.IsAny<TipoPrecio>())).Returns(Task.CompletedTask);
            _unidadTrabajoMock.Setup(u => u.Guardar()).Returns(Task.CompletedTask);
            _unidadTrabajoMock.Setup(u => u.Bitacora.RegistrarBitacora(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Returns(Task.CompletedTask);

            var nuevoTipoPrecio = new TipoPrecio { Id = 0, Descripcion = "Nuevo Tipo Precio", Cambio = 10 };

            
            var result = await _controller.Upsert(nuevoTipoPrecio);

            
            Assert.IsInstanceOf<RedirectToActionResult>(result);
            var redirectResult = result as RedirectToActionResult;
            Assert.NotNull(redirectResult);
            Assert.AreEqual("Index", redirectResult.ActionName);
        }

        [Test]
        public async Task Upsert_Post_Crea_Nuevo_TipoPrecio_Llama_Agregar_Y_Guardar()
        {
            
            _unidadTrabajoMock.Setup(u => u.TipoPrecio.Agregar(It.IsAny<TipoPrecio>())).Returns(Task.CompletedTask);
            _unidadTrabajoMock.Setup(u => u.Guardar()).Returns(Task.CompletedTask);
            _unidadTrabajoMock.Setup(u => u.Bitacora.RegistrarBitacora(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Returns(Task.CompletedTask);

            var nuevoTipoPrecio = new TipoPrecio { Id = 0, Descripcion = "Nuevo Tipo Precio", Cambio = 10 };

            
            await _controller.Upsert(nuevoTipoPrecio);

            
            _unidadTrabajoMock.Verify(u => u.TipoPrecio.Agregar(It.IsAny<TipoPrecio>()), Times.Once);
            _unidadTrabajoMock.Verify(u => u.Guardar(), Times.Exactly(2));
        }

        [Test]
        public async Task Upsert_Post_Crea_Nuevo_TipoPrecio_Registra_Bitacora()
        {
            
            _unidadTrabajoMock.Setup(u => u.TipoPrecio.Agregar(It.IsAny<TipoPrecio>())).Returns(Task.CompletedTask);
            _unidadTrabajoMock.Setup(u => u.Guardar()).Returns(Task.CompletedTask);
            _unidadTrabajoMock.Setup(u => u.Bitacora.RegistrarBitacora(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Returns(Task.CompletedTask);

            var nuevoTipoPrecio = new TipoPrecio { Id = 0, Descripcion = "Nuevo Tipo Precio", Cambio = 10 };

            
            await _controller.Upsert(nuevoTipoPrecio);

            
            _unidadTrabajoMock.Verify(u => u.Bitacora.RegistrarBitacora("testuser", "0", "Se insertó el tipo de precio 'Nuevo Tipo Precio' con ID: 0"), Times.Once);
        }

        [Test]
        public async Task Upsert_Post_Crea_Nuevo_TipoPrecio_Establece_TempData_Exitosa()
        {
            
            _unidadTrabajoMock.Setup(u => u.TipoPrecio.Agregar(It.IsAny<TipoPrecio>())).Returns(Task.CompletedTask);
            _unidadTrabajoMock.Setup(u => u.Guardar()).Returns(Task.CompletedTask);
            _unidadTrabajoMock.Setup(u => u.Bitacora.RegistrarBitacora(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Returns(Task.CompletedTask);

            var nuevoTipoPrecio = new TipoPrecio { Id = 0, Descripcion = "Nuevo Tipo Precio", Cambio = 10 };

            
            await _controller.Upsert(nuevoTipoPrecio);

            
            _tempDataMock.VerifySet(t => t[DS.Exitosa] = "Tipo Precio creado exitosamente", Times.Once);
        }

        [Test]
        public async Task Upsert_Post_Actualiza_TipoPrecio_Existente_Redirecciona_A_Index()
        {
            
            _unidadTrabajoMock.Setup(u => u.TipoPrecio.Actualizar(It.IsAny<TipoPrecio>()));
            _unidadTrabajoMock.Setup(u => u.Guardar()).Returns(Task.CompletedTask);
            _unidadTrabajoMock.Setup(u => u.Bitacora.RegistrarBitacora(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Returns(Task.CompletedTask);

            var existingTipoPrecio = new TipoPrecio { Id = 1, Descripcion = "Tipo Precio Existente", Cambio = 10 };

            
            var result = await _controller.Upsert(existingTipoPrecio);

            
            Assert.IsInstanceOf<RedirectToActionResult>(result);
            var redirectResult = result as RedirectToActionResult;
            Assert.NotNull(redirectResult);
            Assert.AreEqual("Index", redirectResult.ActionName);
        }

        [Test]
        public async Task Upsert_Post_Actualiza_TipoPrecio_Existente_Llama_Actualizar_Y_Guardar()
        {
            
            _unidadTrabajoMock.Setup(u => u.TipoPrecio.Actualizar(It.IsAny<TipoPrecio>()));
            _unidadTrabajoMock.Setup(u => u.Guardar()).Returns(Task.CompletedTask);
            _unidadTrabajoMock.Setup(u => u.Bitacora.RegistrarBitacora(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Returns(Task.CompletedTask);

            var existingTipoPrecio = new TipoPrecio { Id = 1, Descripcion = "Tipo Precio Existente", Cambio = 10 };

            
            await _controller.Upsert(existingTipoPrecio);

            
            _unidadTrabajoMock.Verify(u => u.TipoPrecio.Actualizar(It.IsAny<TipoPrecio>()), Times.Once);
            _unidadTrabajoMock.Verify(u => u.Guardar(), Times.Once);
        }


        [Test]
        public async Task Upsert_Post_Actualiza_TipoPrecio_Existente_Establece_TempData_Exitosa()
        {
            
            _unidadTrabajoMock.Setup(u => u.TipoPrecio.Actualizar(It.IsAny<TipoPrecio>()));
            _unidadTrabajoMock.Setup(u => u.Guardar()).Returns(Task.CompletedTask);
            _unidadTrabajoMock.Setup(u => u.Bitacora.RegistrarBitacora(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Returns(Task.CompletedTask);

            var existingTipoPrecio = new TipoPrecio { Id = 1, Descripcion = "Tipo Precio Existente", Cambio = 10 };

            
            await _controller.Upsert(existingTipoPrecio);

            
            _tempDataMock.VerifySet(t => t[DS.Exitosa] = "Tipo Precio actualizado exitosamente", Times.Once);
        }


        [Test]
        public async Task Eliminar_Post_With_Valid_Id_Returns_JsonResult_With_Success()
        {
            
            int validId = 1;
            var existingTipoPrecio = new TipoPrecio { Id = validId, Descripcion = "Tipo Precio Existente" };
            _unidadTrabajoMock.Setup(u => u.TipoPrecio.Obtener(validId)).ReturnsAsync(existingTipoPrecio);
            _unidadTrabajoMock.Setup(u => u.Bitacora.RegistrarBitacora(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Returns(Task.CompletedTask);


            
            var result = await _controller.Eliminar(validId);

            
            Assert.IsInstanceOf<JsonResult>(result);
            var jsonResult = result as JsonResult;
            Assert.IsNotNull(jsonResult);
            var value = jsonResult.Value as IDictionary<string, object>;
        }

        [Test]
        public async Task Eliminar_Post_Con_Id_Valido_Llama_Remover_Y_Guardar()
        {
            
            int validId = 1;
            var existingTipoPrecio = new TipoPrecio { Id = validId, Descripcion = "Tipo Precio Existente" };

            _unidadTrabajoMock.Setup(u => u.TipoPrecio.Obtener(validId)).ReturnsAsync(existingTipoPrecio);
            _unidadTrabajoMock.Setup(u => u.TipoPrecio.Remover(existingTipoPrecio));
            _unidadTrabajoMock.Setup(u => u.Guardar()).Returns(Task.CompletedTask);
            _unidadTrabajoMock.Setup(u => u.Bitacora.RegistrarBitacora(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Returns(Task.CompletedTask);

            
            await _controller.Eliminar(validId);

            
            _unidadTrabajoMock.Verify(u => u.TipoPrecio.Remover(existingTipoPrecio), Times.Once);
            _unidadTrabajoMock.Verify(u => u.Guardar(), Times.Once);
        }

        [Test]
        public async Task Eliminar_Post_Con_Id_Valido_Registra_Bitacora()
        {
            
            int validId = 1;
            var existingTipoPrecio = new TipoPrecio { Id = validId, Descripcion = "Tipo Precio Existente" };
            _unidadTrabajoMock.Setup(u => u.TipoPrecio.Obtener(validId)).ReturnsAsync(existingTipoPrecio);
            _unidadTrabajoMock.Setup(u => u.Bitacora.RegistrarBitacora(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Returns(Task.CompletedTask);

            
            await _controller.Eliminar(validId);

            
            _unidadTrabajoMock.Verify(u => u.Bitacora.RegistrarBitacora("testuser", validId.ToString(), $"Se eliminó el tipo precio 'Tipo Precio Existente' con ID: {validId}"), Times.Once);
        }


        [Test]
        public async Task Eliminar_Post_con_Id_Invalido_Registera_Error()
        {
            
            int invalidId = 99;
            _unidadTrabajoMock.Setup(u => u.TipoPrecio.Obtener(invalidId)).ReturnsAsync((TipoPrecio)null);
            _unidadTrabajoMock.Setup(u => u.Error.RegistrarError(It.IsAny<string>(), 420)).Returns(Task.CompletedTask);
            _unidadTrabajoMock.Setup(u => u.Bitacora.RegistrarBitacora(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Returns(Task.CompletedTask);


            
            await _controller.Eliminar(invalidId);

            
            _unidadTrabajoMock.Verify(u => u.Error.RegistrarError("Error al borrar el Tipo Precio", 420), Times.Once);
        }





    }
}