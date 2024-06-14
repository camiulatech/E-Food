using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Security.Claims;
using EFood.AccesoDatos.Repositorio.IRepositorio;
using EFood.Modelos;
using EFood.Modelos.ViewModels;
using EFoodCommerce.Areas.Commerce.Controllers;

namespace E_Food.Tests
{
    [TestFixture]
    public class ProductoControllerTests
    {
        private Mock<IUnidadTrabajo> _unidadTrabajoMock;
        private ProductoController _controller;
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
            var identity = new ClaimsIdentity(claims, "mock");
            var user = new ClaimsPrincipal(identity);

            var httpContext = new DefaultHttpContext
            {
                User = user,
                Session = _sessionMock.Object
            };
            _httpContextAccessorMock.Setup(_ => _.HttpContext).Returns(httpContext);

            _controller = new ProductoController(_unidadTrabajoMock.Object)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = httpContext
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
        public async Task Consultar_Con_IdLineaComida_Valido_Retorna_ViewCon_Modelo_ProductoVM()
        {
            int idLineaComida = 1;
            var productos = new List<Producto> { new Producto { Id = 1, Nombre = "Producto de prueba" } };
            _unidadTrabajoMock.Setup(u => u.Producto.FiltrarPorLineaComida(It.IsAny<int>())).ReturnsAsync(productos);

            var resultado = await _controller.Consultar(idLineaComida);

            Assert.IsInstanceOf<ViewResult>(resultado);
            var viewResult = resultado as ViewResult;
            Assert.IsInstanceOf<ProductoVM>(viewResult.Model);
            var modelo = viewResult.Model as ProductoVM;
            Assert.AreEqual(productos, modelo.Productos);
            Assert.AreEqual(idLineaComida, modelo.LineaComidaSeleccionadaId);
        }



    }
}
