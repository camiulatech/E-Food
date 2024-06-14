using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Newtonsoft.Json;
using System.Security.Claims;
using EFood.AccesoDatos.Repositorio.IRepositorio;
using EFood.Modelos.CarritoCompras;
using EFood.Modelos.ViewModels;
using EFoodCommerce.Areas.Commerce.Controllers;

namespace E_Food.Tests
{
    [TestFixture]
    public class CarritoCompraControllerTests
    {
        private Mock<IUnidadTrabajo> _unidadTrabajoMock;
        private CarritoCompraController _controller;
        private Mock<ISession> _sessionMock;
        private Mock<IHttpContextAccessor> _httpContextAccessorMock;

        [SetUp]
        public void Setup()
        {
            _unidadTrabajoMock = new Mock<IUnidadTrabajo>();
            _sessionMock = new Mock<ISession>();
            _httpContextAccessorMock = new Mock<IHttpContextAccessor>();

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, "testuser")
            };
            var identidad = new ClaimsIdentity(claims, "mock");
            var usuario = new ClaimsPrincipal(identidad);

            var contextoHttp = new DefaultHttpContext
            {
                User = usuario,
                Session = _sessionMock.Object
            };
            _httpContextAccessorMock.Setup(_ => _.HttpContext).Returns(contextoHttp);

            _controller = new CarritoCompraController(_unidadTrabajoMock.Object)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = contextoHttp
                }
            };
        }

        [TearDown]
        public void TearDown()
        {
            _controller.Dispose();
        }

        private void SetupSessionGetString(string key, string value)
        {
            byte[] bytes = value == null ? null : System.Text.Encoding.UTF8.GetBytes(value);
            _sessionMock.Setup(s => s.TryGetValue(key, out bytes)).Returns(value != null);
        }

        [Test]
        public void Index_Retorna_ViewResult_Con_CarritoCompra()
        {
            
            var carrito = new CarritoCompra();
            var carritoJson = JsonConvert.SerializeObject(carrito);
            SetupSessionGetString("CarritoCompra", carritoJson);

            
            var resultado = _controller.Index();

            
            Assert.IsInstanceOf<ViewResult>(resultado);
            var viewResult = resultado as ViewResult;
            Assert.NotNull(viewResult);
            Assert.IsInstanceOf<CarritoCompra>(viewResult.Model);
        }

        [Test]
        public void Datos_Retorna_View_Con_Modelo_Cliente()
        {
            
            var resultado = _controller.Datos();

            
            Assert.IsInstanceOf<ViewResult>(resultado);
            var viewResult = resultado as ViewResult;
            Assert.IsInstanceOf<Cliente>(viewResult.Model);
        }

        [Test]
        public async Task MetodoPago_Modelo_Invalido_Retorna_Redireccion_A_Datos()
        {
            
            var cliente = new Cliente();
            _controller.ModelState.AddModelError("Error", "Algún error");

            
            var resultado = await _controller.MetodoPago(cliente);

            
            Assert.IsInstanceOf<RedirectToActionResult>(resultado);
            var redirectResult = resultado as RedirectToActionResult;
            Assert.AreEqual("Datos", redirectResult.ActionName);
        }

        [Test]
        public void DatosPago_Sesion_Valida_Retorna_ViewResult()
        {
            
            var comprasVM = new ComprasVM();
            var comprasVMJson = JsonConvert.SerializeObject(comprasVM);
            SetupSessionGetString("ComprasVM", comprasVMJson);

            
            var resultado = _controller.DatosPago();

            
            Assert.IsInstanceOf<ViewResult>(resultado);
            var viewResult = resultado as ViewResult;
            Assert.IsInstanceOf<ComprasVM>(viewResult.Model);
        }

        [Test]
        public void DatosPago_Sesion_Invalida_Redirecciona_A_Index()
        {
            
            SetupSessionGetString("ComprasVM", null);

            
            var resultado = _controller.DatosPago();

            
            Assert.IsInstanceOf<RedirectToActionResult>(resultado);
            var redirectResult = resultado as RedirectToActionResult;
            Assert.AreEqual("Index", redirectResult.ActionName);
        }

        [Test]
        public void ConfirmarPago_Sesion_Valida_Retorna_ViewResult()
        {
            
            var comprasVM = new ComprasVM();
            var comprasVMJson = JsonConvert.SerializeObject(comprasVM);
            SetupSessionGetString("ComprasVM", comprasVMJson);

            
            var resultado = _controller.ConfirmarPago();

            
            Assert.IsInstanceOf<ViewResult>(resultado);
            var viewResult = resultado as ViewResult;
            Assert.IsInstanceOf<ComprasVM>(viewResult.Model);
        }

        [Test]
        public void ConfirmarPago_Sesion_Invalida_Redirecciona_A_Index()
        {
            
            SetupSessionGetString("ComprasVM", null);

            
            var resultado = _controller.ConfirmarPago();

            
            Assert.IsInstanceOf<RedirectToActionResult>(resultado);
            var redirectResult = resultado as RedirectToActionResult;
            Assert.AreEqual("Index", redirectResult.ActionName);
        }

    }
}
