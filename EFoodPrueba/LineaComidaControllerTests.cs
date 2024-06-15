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
using System.Security.Principal;

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
        public async Task Upsert_Post_Crea_Nueva_LineaComida_Redirecciona_A_Index()
        {
            _unidadTrabajoMock.Setup(u => u.LineaComida.Agregar(It.IsAny<LineaComida>())).Returns(Task.CompletedTask);
            _unidadTrabajoMock.Setup(u => u.Guardar()).Returns(Task.CompletedTask);
            _unidadTrabajoMock.Setup(u => u.Bitacora.RegistrarBitacora(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Returns(Task.CompletedTask);

            var nuevaLineaComida = new LineaComida { Id = 0, Nombre = "Nueva Linea" };

            var result = await _controller.Upsert(nuevaLineaComida);

            Assert.IsInstanceOf<RedirectToActionResult>(result);
            var redirectResult = result as RedirectToActionResult;
            Assert.NotNull(redirectResult);
            Assert.AreEqual("Index", redirectResult.ActionName);
        }


        [Test]
        public async Task Upsert_Post_Crea_Nueva_LineaComida_Llama_Agregar_Y_Guardar()
        {
            _unidadTrabajoMock.Setup(u => u.LineaComida.Agregar(It.IsAny<LineaComida>())).Returns(Task.CompletedTask);
            _unidadTrabajoMock.Setup(u => u.Guardar()).Returns(Task.CompletedTask);
            _unidadTrabajoMock.Setup(u => u.Bitacora.RegistrarBitacora(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Returns(Task.CompletedTask);

            var nuevaLineaComida = new LineaComida { Id = 0, Nombre = "Nueva Linea" };

            await _controller.Upsert(nuevaLineaComida);

            _unidadTrabajoMock.Verify(u => u.LineaComida.Agregar(It.IsAny<LineaComida>()), Times.Once);
            _unidadTrabajoMock.Verify(u => u.Guardar(), Times.Exactly(2));
        }




        [Test]
        public async Task Upsert_Post_Crea_Nueva_LineaComida_Registra_Bitacora()
        {
            _unidadTrabajoMock.Setup(u => u.LineaComida.Agregar(It.IsAny<LineaComida>())).Returns(Task.CompletedTask);
            _unidadTrabajoMock.Setup(u => u.Guardar()).Returns(Task.CompletedTask);
            _unidadTrabajoMock.Setup(u => u.Bitacora.RegistrarBitacora(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Returns(Task.CompletedTask);

            var nuevaLineaComida = new LineaComida { Id = 0, Nombre = "Nueva Linea" };

            await _controller.Upsert(nuevaLineaComida);

            _unidadTrabajoMock.Verify(u => u.Bitacora.RegistrarBitacora("testuser", "0", "Se insertó la línea de comida 'Nueva Linea' con ID: 0"), Times.Once);
        }





        [Test]
        public async Task Upsert_Post_Crea_Nueva_LineaComida_Establece_TempData_Exitosa()
        {
            _unidadTrabajoMock.Setup(u => u.LineaComida.Agregar(It.IsAny<LineaComida>())).Returns(Task.CompletedTask);
            _unidadTrabajoMock.Setup(u => u.Guardar()).Returns(Task.CompletedTask);
            _unidadTrabajoMock.Setup(u => u.Bitacora.RegistrarBitacora(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Returns(Task.CompletedTask);

            var nuevaLineaComida = new LineaComida { Id = 0, Nombre = "Nueva Linea" };

            await _controller.Upsert(nuevaLineaComida);

            _tempDataMock.VerifySet(t => t[DS.Exitosa] = "Linea de Comida creada exitosamente", Times.Once);
        }






        [Test]
        public async Task Upsert_Post_Actualiza_LineaComida_Existente_Redirecciona_A_Index()
        {
            _unidadTrabajoMock.Setup(u => u.LineaComida.Actualizar(It.IsAny<LineaComida>()));
            _unidadTrabajoMock.Setup(u => u.Guardar()).Returns(Task.CompletedTask);
            _unidadTrabajoMock.Setup(u => u.Bitacora.RegistrarBitacora(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Returns(Task.CompletedTask);

            var existingLineaComida = new LineaComida { Id = 1, Nombre = "Linea Existente" };

            var result = await _controller.Upsert(existingLineaComida);

            Assert.IsInstanceOf<RedirectToActionResult>(result);
            var redirectResult = result as RedirectToActionResult;
            Assert.NotNull(redirectResult);
            Assert.AreEqual("Index", redirectResult.ActionName);
        }




        [Test]
        public async Task Upsert_Post_Actualiza_LineaComida_Existente_Llama_Actualizar_Y_Guardar()
        {
            _unidadTrabajoMock.Setup(u => u.LineaComida.Actualizar(It.IsAny<LineaComida>()));
            _unidadTrabajoMock.Setup(u => u.Guardar()).Returns(Task.CompletedTask);
            _unidadTrabajoMock.Setup(u => u.Bitacora.RegistrarBitacora(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Returns(Task.CompletedTask);

            var existingLineaComida = new LineaComida { Id = 1, Nombre = "Linea Existente" };

            await _controller.Upsert(existingLineaComida);

            _unidadTrabajoMock.Verify(u => u.LineaComida.Actualizar(It.IsAny<LineaComida>()), Times.Once);
            _unidadTrabajoMock.Verify(u => u.Guardar(), Times.Once);
        }


        [Test]
        public async Task Upsert_Post_Actualiza_LineaComida_Existente_Registra_Bitacora()
        {
            _unidadTrabajoMock.Setup(u => u.LineaComida.Actualizar(It.IsAny<LineaComida>()));
            _unidadTrabajoMock.Setup(u => u.Guardar()).Returns(Task.CompletedTask);
            _unidadTrabajoMock.Setup(u => u.Bitacora.RegistrarBitacora(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Returns(Task.CompletedTask);

            var existingLineaComida = new LineaComida { Id = 1, Nombre = "Linea Existente" };

            await _controller.Upsert(existingLineaComida);

            _unidadTrabajoMock.Verify(u => u.Bitacora.RegistrarBitacora("testuser", "1", "Se actualizó la línea de comida 'Linea Existente' con ID: 1"), Times.Once);
        }



        [Test]
        public async Task Upsert_Post_Actualiza_LineaComida_Existente_Establece_TempData_Exitosa()
        {
            _unidadTrabajoMock.Setup(u => u.LineaComida.Actualizar(It.IsAny<LineaComida>()));
            _unidadTrabajoMock.Setup(u => u.Guardar()).Returns(Task.CompletedTask);
            _unidadTrabajoMock.Setup(u => u.Bitacora.RegistrarBitacora(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Returns(Task.CompletedTask);

            var existingLineaComida = new LineaComida { Id = 1, Nombre = "Linea Existente" };

            await _controller.Upsert(existingLineaComida);

            _tempDataMock.VerifySet(t => t[DS.Exitosa] = "Linea de Comida actualizada exitosamente", Times.Once);
        }

        [Test]
        public async Task Eliminar_Post_Con_Id_Valido_Retorna_ResultadoJson()
        {

            int validId = 1;
            var existingLineaComida = new LineaComida { Id = validId, Nombre = "Linea Existente" };
            _unidadTrabajoMock.Setup(u => u.LineaComida.Obtener(validId)).ReturnsAsync(existingLineaComida);
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
            var existingLineaComida = new LineaComida { Id = validId, Nombre = "Linea Existente" };
            _unidadTrabajoMock.Setup(u => u.LineaComida.Obtener(validId)).ReturnsAsync(existingLineaComida);
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

   
            _unidadTrabajoMock.Verify(u => u.LineaComida.Remover(existingLineaComida), Times.Once);
            _unidadTrabajoMock.Verify(u => u.Guardar(), Times.Once);
            _unidadTrabajoMock.Verify(u => u.Bitacora.RegistrarBitacora(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Once);
        }



    }
}
