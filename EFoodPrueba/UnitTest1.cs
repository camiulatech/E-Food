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

namespace E_Food.Tests
{
    [TestFixture]
    public class LineaComidaControllerTests
    {
        private Mock<IUnidadTrabajo> _unidadTrabajoMock;
        private Mock<IUserStore<IdentityUser>> _userStoreMock;
        private LineaComidaController _controller;
        private Mock<IUserClaimsPrincipalFactory<IdentityUser>> _userClaimsPrincipalFactoryMock;
        private Mock<ITempDataDictionary> _tempDataMock;

        [SetUp]
        public void Setup()
        {
            _unidadTrabajoMock = new Mock<IUnidadTrabajo>();
            _userStoreMock = new Mock<IUserStore<IdentityUser>>();
            _userClaimsPrincipalFactoryMock = new Mock<IUserClaimsPrincipalFactory<IdentityUser>>();
            _tempDataMock = new Mock<ITempDataDictionary>();

            _controller = new LineaComidaController(_unidadTrabajoMock.Object)
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
        public async Task Upsert_Post_With_Valid_Model_Creates_New_LineaComida()
        {
            // Arrange
            _unidadTrabajoMock.Setup(u => u.LineaComida.Agregar(It.IsAny<LineaComida>())).Returns(Task.CompletedTask);
            _unidadTrabajoMock.Setup(u => u.Guardar()).Returns(Task.CompletedTask);
            _unidadTrabajoMock.Setup(u => u.Bitacora.RegistrarBitacora(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Returns(Task.CompletedTask);

            var nuevaLineaComida = new LineaComida
            {
                Id = 0,
                Nombre = "Nueva Linea"
            };

            // Act
            var result = await _controller.Upsert(nuevaLineaComida);

            // Assert
            Assert.IsInstanceOf<RedirectToActionResult>(result);
            var redirectResult = result as RedirectToActionResult;
            Assert.NotNull(redirectResult);
            Assert.AreEqual("Index", redirectResult.ActionName);
            _unidadTrabajoMock.Verify(u => u.LineaComida.Agregar(It.IsAny<LineaComida>()), Times.Once);
            _unidadTrabajoMock.Verify(u => u.Guardar(), Times.Exactly(2));
            _unidadTrabajoMock.Verify(u => u.Bitacora.RegistrarBitacora("testuser", "0", "Se insertó la línea de comida 'Nueva Linea' con ID: 0"), Times.Once);
            _tempDataMock.VerifySet(t => t[DS.Exitosa] = "Linea de Comida creada exitosamente", Times.Once);
        }

        [Test]
        public async Task Upsert_Post_With_Valid_Model_Updates_Existing_LineaComida()
        {
            // Arrange
            _unidadTrabajoMock.Setup(u => u.LineaComida.Actualizar(It.IsAny<LineaComida>()));
            _unidadTrabajoMock.Setup(u => u.Guardar()).Returns(Task.CompletedTask);
            _unidadTrabajoMock.Setup(u => u.Bitacora.RegistrarBitacora(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Returns(Task.CompletedTask);

            var existingLineaComida = new LineaComida
            {
                Id = 1,
                Nombre = "Linea Existente"
            };

            // Act
            var result = await _controller.Upsert(existingLineaComida);

            // Assert
            Assert.IsInstanceOf<RedirectToActionResult>(result);
            var redirectResult = result as RedirectToActionResult;
            Assert.NotNull(redirectResult);
            Assert.AreEqual("Index", redirectResult.ActionName);
            _unidadTrabajoMock.Verify(u => u.LineaComida.Actualizar(It.IsAny<LineaComida>()), Times.Once);
            _unidadTrabajoMock.Verify(u => u.Guardar(), Times.Once);
            _unidadTrabajoMock.Verify(u => u.Bitacora.RegistrarBitacora("testuser", "1", "Se actualizó la línea de comida 'Linea Existente' con ID: 1"), Times.Once);
            _tempDataMock.VerifySet(t => t[DS.Exitosa] = "Linea de Comida actualizada exitosamente", Times.Once);
        }











        //NUEVAS PRUEBAS--------------------------------------------------------------------------------------------



        [Test]
        public async Task Upsert_Post_Creates_New_LineaComida_Redirects_To_Index()
        {
            // Arrange
            _unidadTrabajoMock.Setup(u => u.LineaComida.Agregar(It.IsAny<LineaComida>())).Returns(Task.CompletedTask);
            _unidadTrabajoMock.Setup(u => u.Guardar()).Returns(Task.CompletedTask);
            _unidadTrabajoMock.Setup(u => u.Bitacora.RegistrarBitacora(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Returns(Task.CompletedTask);

            var nuevaLineaComida = new LineaComida { Id = 0, Nombre = "Nueva Linea" };

            // Act
            var result = await _controller.Upsert(nuevaLineaComida);

            // Assert
            Assert.IsInstanceOf<RedirectToActionResult>(result);
            var redirectResult = result as RedirectToActionResult;
            Assert.NotNull(redirectResult);
            Assert.AreEqual("Index", redirectResult.ActionName);
        }


        [Test]
        public async Task Upsert_Post_Creates_New_LineaComida_Calls_Agregar_And_Guardar()
        {
            // Arrange
            _unidadTrabajoMock.Setup(u => u.LineaComida.Agregar(It.IsAny<LineaComida>())).Returns(Task.CompletedTask);
            _unidadTrabajoMock.Setup(u => u.Guardar()).Returns(Task.CompletedTask);
            _unidadTrabajoMock.Setup(u => u.Bitacora.RegistrarBitacora(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Returns(Task.CompletedTask);

            var nuevaLineaComida = new LineaComida { Id = 0, Nombre = "Nueva Linea" };

            // Act
            await _controller.Upsert(nuevaLineaComida);

            // Assert
            _unidadTrabajoMock.Verify(u => u.LineaComida.Agregar(It.IsAny<LineaComida>()), Times.Once);
            _unidadTrabajoMock.Verify(u => u.Guardar(), Times.Exactly(2));
        }






        [Test]
        public async Task Upsert_Post_Creates_New_LineaComida_Registers_Bitacora()
        {
            // Arrange
            _unidadTrabajoMock.Setup(u => u.LineaComida.Agregar(It.IsAny<LineaComida>())).Returns(Task.CompletedTask);
            _unidadTrabajoMock.Setup(u => u.Guardar()).Returns(Task.CompletedTask);
            _unidadTrabajoMock.Setup(u => u.Bitacora.RegistrarBitacora(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Returns(Task.CompletedTask);

            var nuevaLineaComida = new LineaComida { Id = 0, Nombre = "Nueva Linea" };

            // Act
            await _controller.Upsert(nuevaLineaComida);

            // Assert
            _unidadTrabajoMock.Verify(u => u.Bitacora.RegistrarBitacora("testuser", "0", "Se insertó la línea de comida 'Nueva Linea' con ID: 0"), Times.Once);
        }





        [Test]
        public async Task Upsert_Post_Creates_New_LineaComida_Sets_TempData_Exitosa()
        {
            // Arrange
            _unidadTrabajoMock.Setup(u => u.LineaComida.Agregar(It.IsAny<LineaComida>())).Returns(Task.CompletedTask);
            _unidadTrabajoMock.Setup(u => u.Guardar()).Returns(Task.CompletedTask);
            _unidadTrabajoMock.Setup(u => u.Bitacora.RegistrarBitacora(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Returns(Task.CompletedTask);

            var nuevaLineaComida = new LineaComida { Id = 0, Nombre = "Nueva Linea" };

            // Act
            await _controller.Upsert(nuevaLineaComida);

            // Assert
            _tempDataMock.VerifySet(t => t[DS.Exitosa] = "Linea de Comida creada exitosamente", Times.Once);
        }






        [Test]
        public async Task Upsert_Post_Updates_Existing_LineaComida_Redirects_To_Index()
        {
            // Arrange
            _unidadTrabajoMock.Setup(u => u.LineaComida.Actualizar(It.IsAny<LineaComida>()));
            _unidadTrabajoMock.Setup(u => u.Guardar()).Returns(Task.CompletedTask);
            _unidadTrabajoMock.Setup(u => u.Bitacora.RegistrarBitacora(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Returns(Task.CompletedTask);

            var existingLineaComida = new LineaComida { Id = 1, Nombre = "Linea Existente" };

            // Act
            var result = await _controller.Upsert(existingLineaComida);

            // Assert
            Assert.IsInstanceOf<RedirectToActionResult>(result);
            var redirectResult = result as RedirectToActionResult;
            Assert.NotNull(redirectResult);
            Assert.AreEqual("Index", redirectResult.ActionName);
        }




        [Test]
        public async Task Upsert_Post_Updates_Existing_LineaComida_Calls_Actualizar_And_Guardar()
        {
            // Arrange
            _unidadTrabajoMock.Setup(u => u.LineaComida.Actualizar(It.IsAny<LineaComida>()));
            _unidadTrabajoMock.Setup(u => u.Guardar()).Returns(Task.CompletedTask);
            _unidadTrabajoMock.Setup(u => u.Bitacora.RegistrarBitacora(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Returns(Task.CompletedTask);

            var existingLineaComida = new LineaComida { Id = 1, Nombre = "Linea Existente" };

            // Act
            await _controller.Upsert(existingLineaComida);

            // Assert
            _unidadTrabajoMock.Verify(u => u.LineaComida.Actualizar(It.IsAny<LineaComida>()), Times.Once);
            _unidadTrabajoMock.Verify(u => u.Guardar(), Times.Once);
        }


        [Test]
        public async Task Upsert_Post_Updates_Existing_LineaComida_Registers_Bitacora()
        {
            // Arrange
            _unidadTrabajoMock.Setup(u => u.LineaComida.Actualizar(It.IsAny<LineaComida>()));
            _unidadTrabajoMock.Setup(u => u.Guardar()).Returns(Task.CompletedTask);
            _unidadTrabajoMock.Setup(u => u.Bitacora.RegistrarBitacora(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Returns(Task.CompletedTask);

            var existingLineaComida = new LineaComida { Id = 1, Nombre = "Linea Existente" };

            // Act
            await _controller.Upsert(existingLineaComida);

            // Assert
            _unidadTrabajoMock.Verify(u => u.Bitacora.RegistrarBitacora("testuser", "1", "Se actualizó la línea de comida 'Linea Existente' con ID: 1"), Times.Once);
        }



        [Test]
        public async Task Upsert_Post_Updates_Existing_LineaComida_Sets_TempData_Exitosa()
        {
            // Arrange
            _unidadTrabajoMock.Setup(u => u.LineaComida.Actualizar(It.IsAny<LineaComida>()));
            _unidadTrabajoMock.Setup(u => u.Guardar()).Returns(Task.CompletedTask);
            _unidadTrabajoMock.Setup(u => u.Bitacora.RegistrarBitacora(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Returns(Task.CompletedTask);

            var existingLineaComida = new LineaComida { Id = 1, Nombre = "Linea Existente" };

            // Act
            await _controller.Upsert(existingLineaComida);

            // Assert
            _tempDataMock.VerifySet(t => t[DS.Exitosa] = "Linea de Comida actualizada exitosamente", Times.Once);
        }
    }

}
