using System.Security.Claims;
using System.Threading.Tasks;
using E_Food.Areas.Admin.Controllers;
using EFood.AccesoDatos.Repositorio.IRepositorio;
using EFood.Modelos;
using EFood.Utilidades;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System.Security.Principal;

namespace E_Food.Tests
{
    [TestFixture]
    public class TipoPrecioControllerTests
    {
        private Mock<IUnidadTrabajo> _unidadTrabajoMock;
        private TipoPrecioController _controller;
        private Mock<ITempDataDictionary> _tempDataMock;

        [SetUp]
        public void Setup()
        {
            _unidadTrabajoMock = new Mock<IUnidadTrabajo>();
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
            // Arrange
            _unidadTrabajoMock.Setup(u => u.TipoPrecio.Agregar(It.IsAny<TipoPrecio>())).Returns(Task.CompletedTask);
            _unidadTrabajoMock.Setup(u => u.Guardar()).Returns(Task.CompletedTask);
            _unidadTrabajoMock.Setup(u => u.Bitacora.RegistrarBitacora(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Returns(Task.CompletedTask);

            var nuevoTipoPrecio = new TipoPrecio { Id = 0, Descripcion = "Nuevo Tipo Precio", Cambio = 10 };

            // Act
            var result = await _controller.Upsert(nuevoTipoPrecio);

            // Assert
            Assert.IsInstanceOf<RedirectToActionResult>(result);
            var redirectResult = result as RedirectToActionResult;
            Assert.NotNull(redirectResult);
            Assert.AreEqual("Index", redirectResult.ActionName);
        }

        [Test]
        public async Task Upsert_Post_Crea_Nuevo_TipoPrecio_Llama_Agregar_Y_Guardar()
        {
            // Arrange
            _unidadTrabajoMock.Setup(u => u.TipoPrecio.Agregar(It.IsAny<TipoPrecio>())).Returns(Task.CompletedTask);
            _unidadTrabajoMock.Setup(u => u.Guardar()).Returns(Task.CompletedTask);
            _unidadTrabajoMock.Setup(u => u.Bitacora.RegistrarBitacora(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Returns(Task.CompletedTask);

            var nuevoTipoPrecio = new TipoPrecio { Id = 0, Descripcion = "Nuevo Tipo Precio", Cambio = 10 };

            // Act
            await _controller.Upsert(nuevoTipoPrecio);

            // Assert
            _unidadTrabajoMock.Verify(u => u.TipoPrecio.Agregar(It.IsAny<TipoPrecio>()), Times.Once);
            _unidadTrabajoMock.Verify(u => u.Guardar(), Times.Exactly(2));
        }

        [Test]
        public async Task Upsert_Post_Crea_Nuevo_TipoPrecio_Registra_Bitacora()
        {
            // Arrange
            _unidadTrabajoMock.Setup(u => u.TipoPrecio.Agregar(It.IsAny<TipoPrecio>())).Returns(Task.CompletedTask);
            _unidadTrabajoMock.Setup(u => u.Guardar()).Returns(Task.CompletedTask);
            _unidadTrabajoMock.Setup(u => u.Bitacora.RegistrarBitacora(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Returns(Task.CompletedTask);

            var nuevoTipoPrecio = new TipoPrecio { Id = 0, Descripcion = "Nuevo Tipo Precio", Cambio = 10 };

            // Act
            await _controller.Upsert(nuevoTipoPrecio);

            // Assert
            _unidadTrabajoMock.Verify(u => u.Bitacora.RegistrarBitacora("testuser", "0", "Se insertó el tipo de precio 'Nuevo Tipo Precio' con ID: 0"), Times.Once);
        }

        [Test]
        public async Task Upsert_Post_Crea_Nuevo_TipoPrecio_Establece_TempData_Exitosa()
        {
            // Arrange
            _unidadTrabajoMock.Setup(u => u.TipoPrecio.Agregar(It.IsAny<TipoPrecio>())).Returns(Task.CompletedTask);
            _unidadTrabajoMock.Setup(u => u.Guardar()).Returns(Task.CompletedTask);
            _unidadTrabajoMock.Setup(u => u.Bitacora.RegistrarBitacora(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Returns(Task.CompletedTask);

            var nuevoTipoPrecio = new TipoPrecio { Id = 0, Descripcion = "Nuevo Tipo Precio", Cambio = 10 };

            // Act
            await _controller.Upsert(nuevoTipoPrecio);

            // Assert
            _tempDataMock.VerifySet(t => t[DS.Exitosa] = "Tipo Precio creado exitosamente", Times.Once);
        }

        [Test]
        public async Task Upsert_Post_Actualiza_TipoPrecio_Existente_Redirecciona_A_Index()
        {
            // Arrange
            _unidadTrabajoMock.Setup(u => u.TipoPrecio.Actualizar(It.IsAny<TipoPrecio>()));
            _unidadTrabajoMock.Setup(u => u.Guardar()).Returns(Task.CompletedTask);
            _unidadTrabajoMock.Setup(u => u.Bitacora.RegistrarBitacora(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Returns(Task.CompletedTask);

            var existingTipoPrecio = new TipoPrecio { Id = 1, Descripcion = "Tipo Precio Existente", Cambio = 10 };

            // Act
            var result = await _controller.Upsert(existingTipoPrecio);

            // Assert
            Assert.IsInstanceOf<RedirectToActionResult>(result);
            var redirectResult = result as RedirectToActionResult;
            Assert.NotNull(redirectResult);
            Assert.AreEqual("Index", redirectResult.ActionName);
        }

        [Test]
        public async Task Upsert_Post_Actualiza_TipoPrecio_Existente_Llama_Actualizar_Y_Guardar()
        {
            // Arrange
            _unidadTrabajoMock.Setup(u => u.TipoPrecio.Actualizar(It.IsAny<TipoPrecio>()));
            _unidadTrabajoMock.Setup(u => u.Guardar()).Returns(Task.CompletedTask);
            _unidadTrabajoMock.Setup(u => u.Bitacora.RegistrarBitacora(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Returns(Task.CompletedTask);

            var existingTipoPrecio = new TipoPrecio { Id = 1, Descripcion = "Tipo Precio Existente", Cambio = 10 };

            // Act
            await _controller.Upsert(existingTipoPrecio);

            // Assert
            _unidadTrabajoMock.Verify(u => u.TipoPrecio.Actualizar(It.IsAny<TipoPrecio>()), Times.Once);
            _unidadTrabajoMock.Verify(u => u.Guardar(), Times.Once);
        }

        [Test]
        public async Task Upsert_Post_Actualiza_TipoPrecio_Existente_Registra_Bitacora()
        {
            // Arrange
            _unidadTrabajoMock.Setup(u => u.TipoPrecio.Actualizar(It.IsAny<TipoPrecio>()));
            _unidadTrabajoMock.Setup(u => u.Guardar()).Returns(Task.CompletedTask);
            _unidadTrabajoMock.Setup(u => u.Bitacora.RegistrarBitacora(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Returns(Task.CompletedTask);

            var existingTipoPrecio = new TipoPrecio { Id = 1, Descripcion = "Tipo Precio Existente", Cambio = 10 };

            // Act
            await _controller.Upsert(existingTipoPrecio);

            // Assert
            _unidadTrabajoMock.Verify(u => u.Bitacora.RegistrarBitacora("testuser", "1", "Se actualizó el tipo de precio 'Tipo Precio Existente' con ID: 1"), Times.Once);

        }


        [Test]
        public async Task Upsert_Post_Actualiza_TipoPrecio_Existente_Establece_TempData_Exitosa()
        {
            // Arrange
            _unidadTrabajoMock.Setup(u => u.TipoPrecio.Actualizar(It.IsAny<TipoPrecio>()));
            _unidadTrabajoMock.Setup(u => u.Guardar()).Returns(Task.CompletedTask);
            _unidadTrabajoMock.Setup(u => u.Bitacora.RegistrarBitacora(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Returns(Task.CompletedTask);

            var existingTipoPrecio = new TipoPrecio { Id = 1, Descripcion = "Tipo Precio Existente", Cambio = 10 };

            // Act
            await _controller.Upsert(existingTipoPrecio);

            // Assert
            _tempDataMock.VerifySet(t => t[DS.Exitosa] = "Tipo Precio actualizado exitosamente", Times.Once);
        }


        [Test]
        public async Task Eliminar_Post_With_Valid_Id_Returns_JsonResult_With_Success()
        {
            // Arrange
            int validId = 1;
            var existingTipoPrecio = new TipoPrecio { Id = validId, Descripcion = "Tipo Precio Existente" };
            _unidadTrabajoMock.Setup(u => u.TipoPrecio.Obtener(validId)).ReturnsAsync(existingTipoPrecio);
            _unidadTrabajoMock.Setup(u => u.Bitacora.RegistrarBitacora(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Returns(Task.CompletedTask);


            // Act
            var result = await _controller.Eliminar(validId);

            // Assert
            Assert.IsInstanceOf<JsonResult>(result);
            var jsonResult = result as JsonResult;
            Assert.IsNotNull(jsonResult);
            var value = jsonResult.Value as IDictionary<string, object>;
        }

        [Test]
        public async Task Eliminar_Post_With_Valid_Id_Calls_Remover_And_Guardar()
        {
            // Arrange
            int validId = 1;
            var existingTipoPrecio = new TipoPrecio { Id = validId, Descripcion = "Tipo Precio Existente" };

            _unidadTrabajoMock.Setup(u => u.TipoPrecio.Obtener(validId)).ReturnsAsync(existingTipoPrecio);
            _unidadTrabajoMock.Setup(u => u.TipoPrecio.Remover(existingTipoPrecio));
            _unidadTrabajoMock.Setup(u => u.Guardar()).Returns(Task.CompletedTask);
            _unidadTrabajoMock.Setup(u => u.Bitacora.RegistrarBitacora(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Returns(Task.CompletedTask);

            // Act
            await _controller.Eliminar(validId);

            // Assert
            _unidadTrabajoMock.Verify(u => u.TipoPrecio.Remover(existingTipoPrecio), Times.Once);
            _unidadTrabajoMock.Verify(u => u.Guardar(), Times.Once);
        }

        [Test]
        public async Task Eliminar_Post_With_Valid_Id_Registers_Bitacora()
        {
            // Arrange
            int validId = 1;
            var existingTipoPrecio = new TipoPrecio { Id = validId, Descripcion = "Tipo Precio Existente" };
            _unidadTrabajoMock.Setup(u => u.TipoPrecio.Obtener(validId)).ReturnsAsync(existingTipoPrecio);
            _unidadTrabajoMock.Setup(u => u.Bitacora.RegistrarBitacora(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Returns(Task.CompletedTask);

            // Act
            await _controller.Eliminar(validId);

            // Assert
            _unidadTrabajoMock.Verify(u => u.Bitacora.RegistrarBitacora("testuser", validId.ToString(), $"Se eliminó el tipo precio 'Tipo Precio Existente' con ID: {validId}"), Times.Once);
        }

        [Test]
        public async Task Eliminar_Post_With_Invalid_Id_Returns_JsonResult_With_Error()
        {
            // Arrange
            int invalidId = 99;
            _unidadTrabajoMock.Setup(u => u.TipoPrecio.Obtener(invalidId)).ReturnsAsync((TipoPrecio)null);
            _unidadTrabajoMock.Setup(u => u.Bitacora.RegistrarBitacora(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Returns(Task.CompletedTask);


            // Act
            var result = await _controller.Eliminar(invalidId);

            // Assert
            Assert.IsInstanceOf<JsonResult>(result);
            var jsonResult = result as JsonResult;
            Assert.IsNotNull(jsonResult);
            var value = jsonResult.Value as IDictionary<string, object>;
            Assert.IsFalse((bool)value["success"]);
            Assert.AreEqual("Error al borrar el Tipo Precio", value["mensajeError"]);
        }

        [Test]
        public async Task Eliminar_Post_With_Invalid_Id_Registers_Error()
        {
            // Arrange
            int invalidId = 99;
            _unidadTrabajoMock.Setup(u => u.TipoPrecio.Obtener(invalidId)).ReturnsAsync((TipoPrecio)null);
            _unidadTrabajoMock.Setup(u => u.Error.RegistrarError(It.IsAny<string>(), 420)).Returns(Task.CompletedTask);
            _unidadTrabajoMock.Setup(u => u.Bitacora.RegistrarBitacora(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Returns(Task.CompletedTask);


            // Act
            await _controller.Eliminar(invalidId);

            // Assert
            _unidadTrabajoMock.Verify(u => u.Error.RegistrarError("Error al borrar el Tipo Precio", 420), Times.Once);
        }





    }
}