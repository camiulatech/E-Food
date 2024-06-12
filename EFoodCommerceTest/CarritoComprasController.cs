using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Newtonsoft.Json;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using EFood.AccesoDatos.Repositorio.IRepositorio;
using EFood.Modelos;
using EFood.Modelos.CarritoCompras;
using EFood.Modelos.ViewModels;
using EFood.Utilidades;
using EFoodCommerce.Areas.Commerce.Controllers;
using System.Linq.Expressions;

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

        [Test]
        public void Index_Retorna_ViewResult_Con_CarritoCompra()
        {
            // Preparar
            var carrito = new CarritoCompra();
            var carritoJson = JsonConvert.SerializeObject(carrito);
            _sessionMock.Setup(s => s.GetString(It.IsAny<string>())).Returns(carritoJson);

            // Actuar
            var resultado = _controller.Index();

            // Afirmar
            Assert.IsInstanceOf<ViewResult>(resultado);
            var viewResult = resultado as ViewResult;
            Assert.NotNull(viewResult);
            Assert.IsInstanceOf<CarritoCompra>(viewResult.Model);
        }

        //[Test]
        //public async Task ActualizarCantidad_Actualiza_Cantidad_Retorna_JsonResult()
        //{
        //    // Preparar
        //    int productoId = 1;
        //    int tipoPrecioId = 1;
        //    int cantidad = 5;

        //    var producto = new Producto { Id = productoId };
        //    var tipoPrecio = new TipoPrecio { Id = tipoPrecioId };

        //    _unidadTrabajoMock.Setup(u => u.Producto.ObtenerPrimero(It.IsAny<Expression<Func<Producto, bool>>>()))
        //                      .ReturnsAsync(producto);
        //    _unidadTrabajoMock.Setup(u => u.TipoPrecio.ObtenerPrimero(It.IsAny<Expression<Func<TipoPrecio, bool>>>()))
        //                      .ReturnsAsync(tipoPrecio);

        //    var carrito = new CarritoCompra();
        //    var carritoJson = JsonConvert.SerializeObject(carrito);
        //    _sessionMock.Setup(s => s.GetString(It.IsAny<string>())).Returns(carritoJson);

        //    // Actuar
        //    var resultado = await _controller.ActualizarCantidad(productoId, tipoPrecioId, cantidad);

        //    // Afirmar
        //    Assert.IsInstanceOf<JsonResult>(resultado);
        //    var jsonResult = resultado as JsonResult;
        //    Assert.NotNull(jsonResult);
        //    dynamic jsonData = jsonResult.Value;
        //    Assert.IsTrue(jsonData.success);
        //}

        //[Test]
        //public async Task ActualizarCantidad_Producto_Invalido_Retorna_Error_JsonResult()
        //{
        //    // Preparar
        //    int productoId = 1;
        //    int tipoPrecioId = 1;
        //    int cantidad = 5;

        //    _unidadTrabajoMock.Setup(u => u.Producto.ObtenerPrimero(It.IsAny<Expression<Func<Producto, bool>>>()))
        //                      .ReturnsAsync((Producto)null);

        //    // Actuar
        //    var resultado = await _controller.ActualizarCantidad(productoId, tipoPrecioId, cantidad);

        //    // Afirmar
        //    Assert.IsInstanceOf<JsonResult>(resultado);
        //    var jsonResult = resultado as JsonResult;
        //    Assert.NotNull(jsonResult);
        //    dynamic jsonData = jsonResult.Value;
        //    Assert.IsFalse(jsonData.success);
        //}

        [Test]
        public void Datos_Retorna_View_Con_Modelo_Cliente()
        {
            // Actuar
            var resultado = _controller.Datos();

            // Afirmar
            Assert.IsInstanceOf<ViewResult>(resultado);
            var viewResult = resultado as ViewResult;
            Assert.IsInstanceOf<Cliente>(viewResult.Model);
        }

        //[Test]
        //public async Task MetodoPago_Modelo_Valido_Redirecciona_A_MetodoPagoView()
        //{
        //    // Preparar
        //    var cliente = new Cliente { Nombre = "Test", TiqueteDescuento = "CODIGOVALIDO" };
        //    var tiqueteDescuento = new TiqueteDescuento { Codigo = "CODIGOVALIDO", Disponibles = 1 };

        //    _unidadTrabajoMock.Setup(u => u.TiqueteDescuento.ObtenerPrimero(It.IsAny<Expression<Func<TiqueteDescuento, bool>>>()))
        //                      .ReturnsAsync(tiqueteDescuento);

        //    // Actuar
        //    var resultado = await _controller.MetodoPago(cliente);

        //    // Afirmar
        //    Assert.IsInstanceOf<ViewResult>(resultado);
        //    var viewResult = resultado as ViewResult;
        //    Assert.IsInstanceOf<ComprasVM>(viewResult.Model);
        //}

        [Test]
        public async Task MetodoPago_Modelo_Invalido_Retorna_Redireccion_A_Datos()
        {
            // Preparar
            var cliente = new Cliente();
            _controller.ModelState.AddModelError("Error", "Algún error");

            // Actuar
            var resultado = await _controller.MetodoPago(cliente);

            // Afirmar
            Assert.IsInstanceOf<RedirectToActionResult>(resultado);
            var redirectResult = resultado as RedirectToActionResult;
            Assert.AreEqual("Datos", redirectResult.ActionName);
        }

        [Test]
        public void DatosPago_Sesion_Valida_Retorna_ViewResult()
        {
            // Preparar
            var comprasVM = new ComprasVM();
            var comprasVMJson = JsonConvert.SerializeObject(comprasVM);
            _sessionMock.Setup(s => s.GetString("ComprasVM")).Returns(comprasVMJson);

            // Actuar
            var resultado = _controller.DatosPago();

            // Afirmar
            Assert.IsInstanceOf<ViewResult>(resultado);
            var viewResult = resultado as ViewResult;
            Assert.IsInstanceOf<ComprasVM>(viewResult.Model);
        }

        [Test]
        public void DatosPago_Sesion_Invalida_Redirecciona_A_Index()
        {
            // Preparar
            _sessionMock.Setup(s => s.GetString("ComprasVM")).Returns((string)null);

            // Actuar
            var resultado = _controller.DatosPago();

            // Afirmar
            Assert.IsInstanceOf<RedirectToActionResult>(resultado);
            var redirectResult = resultado as RedirectToActionResult;
            Assert.AreEqual("Index", redirectResult.ActionName);
        }

        [Test]
        public void ConfirmarPago_Sesion_Valida_Retorna_ViewResult()
        {
            // Preparar
            var comprasVM = new ComprasVM();
            var comprasVMJson = JsonConvert.SerializeObject(comprasVM);
            _sessionMock.Setup(s => s.GetString("ComprasVM")).Returns(comprasVMJson);

            // Actuar
            var resultado = _controller.ConfirmarPago();

            // Afirmar
            Assert.IsInstanceOf<ViewResult>(resultado);
            var viewResult = resultado as ViewResult;
            Assert.IsInstanceOf<ComprasVM>(viewResult.Model);
        }

        [Test]
        public void ConfirmarPago_Sesion_Invalida_Redirecciona_A_Index()
        {
            // Preparar
            _sessionMock.Setup(s => s.GetString("ComprasVM")).Returns((string)null);

            // Actuar
            var resultado = _controller.ConfirmarPago();

            // Afirmar
            Assert.IsInstanceOf<RedirectToActionResult>(resultado);
            var redirectResult = resultado as RedirectToActionResult;
            Assert.AreEqual("Index", redirectResult.ActionName);
        }
    }
}
