using System.Security.Claims;
using System.Threading.Tasks;
using E_Food.Areas.Admin.Controllers;
using EFood.AccesoDatos.Repositorio.IRepositorio;
using EFood.Modelos;
using EFood.Modelos.ViewModels;
using EFood.Utilidades;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Moq;
using NUnit.Framework;

namespace E_Food.Tests
{
    [TestFixture]
    public class ProductoPrecioControllerTests
    {
        private Mock<IUnidadTrabajo> _unidadTrabajoMock;
        private ProductoPrecioController _controller;
        private Mock<ITempDataDictionary> _tempDataMock;

        [SetUp]
        public void Setup()
        {
            _unidadTrabajoMock = new Mock<IUnidadTrabajo>();
            _tempDataMock = new Mock<ITempDataDictionary>();

            _controller = new ProductoPrecioController(_unidadTrabajoMock.Object)
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

        //[Test]
        //public async Task Upsert_Post_Crea_Nuevo_Precio_Redirecciona_A_Index()
        //{
        //    // Arrange
        //    _unidadTrabajoMock.Setup(u => u.Producto.AgregarPrecio(It.IsAny<Producto>(), It.IsAny<TipoPrecio>()));
        //    _unidadTrabajoMock.Setup(u => u.Guardar()).Returns(Task.CompletedTask);
        //    _unidadTrabajoMock.Setup(u => u.Bitacora.RegistrarBitacora(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Returns(Task.CompletedTask);

        //    var nuevoProducto = new Producto
        //    {
        //        Id = 1, // Establece un ID válido para el producto
        //                // Configura otros valores necesarios para el producto
        //    };

        //    var nuevoTipoPrecio = new TipoPrecio
        //    {
        //        Id = 1, // Establece un ID válido para el tipo de precio
        //                // Configura otros valores necesarios para el tipo de precio
        //    };

        //    var nuevoProductoPrecioVM = new ProductoPrecioVM
        //    {
        //        Producto = nuevoProducto,
        //        TipoPrecio = nuevoTipoPrecio
        //    };

        //    // Act
        //    var result = await _controller.Upsert(nuevoProductoPrecioVM);

        //    // Assert
        //    Assert.IsInstanceOf<RedirectToActionResult>(result);
        //    var redirectResult = result as RedirectToActionResult;
        //    Assert.NotNull(redirectResult);
        //    Assert.AreEqual("Index", redirectResult.ActionName);
        //}

    }

}
