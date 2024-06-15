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
using System.Linq.Expressions;

namespace E_Food.Tests
{
    [TestFixture]
    public class ProcesadorTarjetaControllerTests
    {
        private Mock<IUnidadTrabajo> _unidadTrabajoMock;
        private Mock<IUserStore<IdentityUser>> _userStoreMock;
        private ProcesadorTarjetaController _controller;
        private Mock<IUserClaimsPrincipalFactory<IdentityUser>> _userClaimsPrincipalFactoryMock;
        private Mock<ITempDataDictionary> _tempDataMock;

        [SetUp]
        public void Setup()
        {
            _unidadTrabajoMock = new Mock<IUnidadTrabajo>();
            _userStoreMock = new Mock<IUserStore<IdentityUser>>();
            _userClaimsPrincipalFactoryMock = new Mock<IUserClaimsPrincipalFactory<IdentityUser>>();
            _tempDataMock = new Mock<ITempDataDictionary>();

            _controller = new ProcesadorTarjetaController(_unidadTrabajoMock.Object)
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
        public async Task Index_IdEsNulo_RetornaVista()
        {
            
            _unidadTrabajoMock.Setup(repo => repo.ProcesadorPago.ObtenerPrimero(It.IsAny<Expression<Func<ProcesadorPago, bool>>>(), null, true))
                .ReturnsAsync(new ProcesadorPago { Tipo = TipoProcesadorPago.TarjetaDebitoCredito });

            
            var result = await _controller.Index(null);

            
            Assert.IsInstanceOf<ViewResult>(result);
        }

        [Test]
        public async Task Index_IdValido_ProcesadorPagoEsNulo_RedireccionaAIndexDeProcesadorPago()
        {
            
            _unidadTrabajoMock.Setup(repo => repo.ProcesadorPago.ObtenerPrimero(It.IsAny<Expression<Func<ProcesadorPago, bool>>>(), null, true))
                .ReturnsAsync((ProcesadorPago)null);

            
            var result = await _controller.Index(1);

            
            var redirectToActionResult = result as RedirectResult;
            Assert.IsNotNull(redirectToActionResult);
            Assert.AreEqual("/Admin/ProcesadorPago/Index", redirectToActionResult.Url);
        }


        [Test]
        public async Task Index_IdValido_ProcesadorPagoEsNulo_EstableceErrorEnTempData()
        {
            
            _unidadTrabajoMock.Setup(repo => repo.ProcesadorPago.ObtenerPrimero(It.IsAny<Expression<Func<ProcesadorPago, bool>>>(), null, true))
                .ReturnsAsync((ProcesadorPago)null);

            
            await _controller.Index(1);

            
            _tempDataMock.VerifySet(t => t[DS.Error] = "Para asignar tarjetas el procesador debe ser de tipo Tarjeta Crédito/Débito!", Times.Once);
        }

        [Test]
        public async Task Index_IdValido_ProcesadorPagoNoEsNulo_RetornaVista()
        {
            
            var procesadorPago = new ProcesadorPago { Id = 1, Tipo = TipoProcesadorPago.TarjetaDebitoCredito };
            _unidadTrabajoMock.Setup(repo => repo.ProcesadorPago.ObtenerPrimero(It.IsAny<Expression<Func<ProcesadorPago, bool>>>(), null, true))
                .ReturnsAsync(procesadorPago);

            
            var result = await _controller.Index(1);

            
            var viewResult = result as ViewResult;
            Assert.IsNotNull(viewResult);
        }

        [Test]
        public async Task ObtenerTodos_IdValido_RetornaJsonResult()
        {
            
            var procesador = new ProcesadorPago
            {
                Id = 1,
                Tipo = TipoProcesadorPago.TarjetaDebitoCredito,
                Tarjetas = new List<Tarjeta> { new Tarjeta { Id = 1, Nombre = "Tarjeta1" } }
            };
            _unidadTrabajoMock.Setup(repo => repo.ProcesadorPago.ObtenerPrimero(It.IsAny<Expression<Func<ProcesadorPago, bool>>>(), "Tarjetas", true))
                .ReturnsAsync(procesador);

            
            var result = await _controller.ObtenerTodos(1);

            
            var jsonResult = result as JsonResult;
            Assert.IsNotNull(jsonResult);
        }


        [Test]
        public async Task ObtenerNoAsociados_IdValido_RetornaJsonResult()
        {
            
            var procesador = new ProcesadorPago
            {
                Id = 1,
                Tipo = TipoProcesadorPago.TarjetaDebitoCredito,
                Tarjetas = new List<Tarjeta> { new Tarjeta { Id = 1, Nombre = "Tarjeta1" } }
            };

            var tarjetasNoAsociadas = new List<Tarjeta> { new Tarjeta { Id = 2, Nombre = "Tarjeta2" } };

            _unidadTrabajoMock.Setup(repo => repo.ProcesadorPago.ObtenerPrimero(It.IsAny<Expression<Func<ProcesadorPago, bool>>>(), "Tarjetas", true))
                .ReturnsAsync(procesador);

            _unidadTrabajoMock.Setup(repo => repo.Tarjeta.ObtenerTodos(It.IsAny<Expression<Func<Tarjeta, bool>>>(), It.IsAny<Func<IQueryable<Tarjeta>, IOrderedQueryable<Tarjeta>>>(), null, true))
                .ReturnsAsync(tarjetasNoAsociadas);

            
            var result = await _controller.ObtenerNoAsociados(1);

            
            var jsonResult = result as JsonResult;
            Assert.IsNotNull(jsonResult);
            var data = jsonResult.Value as dynamic;
            Assert.IsNotNull(data);
        }


        [Test]
        public async Task Upsert_Post_Llama_AgregarTarjeta()
        {
            _unidadTrabajoMock.Setup(u => u.Bitacora.RegistrarBitacora(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Returns(Task.CompletedTask);

            
            var procesador = new ProcesadorPago
            {
                Id = 1,
                Tarjetas = new List<Tarjeta>()
            };
            var tarjeta = new Tarjeta { Id = 2, Nombre = "Tarjeta2" };

            _unidadTrabajoMock.Setup(repo => repo.ProcesadorPago.ObtenerPrimero(It.IsAny<Expression<Func<ProcesadorPago, bool>>>(), "Tarjetas", true))
                .ReturnsAsync(procesador);
            _unidadTrabajoMock.Setup(repo => repo.Tarjeta.Obtener(It.IsAny<int>())).ReturnsAsync(tarjeta);
            _unidadTrabajoMock.Setup(repo => repo.Guardar()).Returns(Task.CompletedTask);

            
            await _controller.Upsert("1,2");

            
            _unidadTrabajoMock.Verify(repo => repo.ProcesadorPago.AgregarTarjeta(procesador, tarjeta), Times.Once);
        }

        [Test]
        public async Task Eliminar_Post_Con_IdValido_Retorna_JsonResult()
        {
            
            int idProcesador = 1;
            int idTarjeta = 2;
            var procesadorPago = new ProcesadorPago { Id = idProcesador, Tarjetas = new List<Tarjeta> { new Tarjeta { Id = idTarjeta, Nombre = "Tarjeta1" } } };
            var tarjeta = new Tarjeta { Id = idTarjeta, Nombre = "Tarjeta1" };

            _unidadTrabajoMock.Setup(u => u.ProcesadorPago.Obtener(idProcesador)).ReturnsAsync(procesadorPago);
            _unidadTrabajoMock.Setup(u => u.Tarjeta.Obtener(idTarjeta)).ReturnsAsync(tarjeta);
            _unidadTrabajoMock.Setup(u => u.Guardar()).Returns(Task.CompletedTask);
            _unidadTrabajoMock.Setup(u => u.Bitacora.RegistrarBitacora(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Returns(Task.CompletedTask);

            
            var result = await _controller.Eliminar($"{idProcesador},{idTarjeta}");

            
            var jsonResult = result as JsonResult;
            Assert.IsNotNull(jsonResult);
            _unidadTrabajoMock.Verify(u => u.ProcesadorPago.RemoverTarjeta(procesadorPago, tarjeta), Times.Once);
            _unidadTrabajoMock.Verify(u => u.Guardar(), Times.Once);
            _unidadTrabajoMock.Verify(u => u.Bitacora.RegistrarBitacora(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Once);
        }
    }
}
