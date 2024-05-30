using E_Food.Areas.Admin.Controllers;
using EFood.AccesoDatos.Data;
using EFood.AccesoDatos.Repositorio.IRepositorio;
using EFood.Modelos;
using EFood.Utilidades;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework;
using System.Security.Claims;
using System.Threading.Tasks;

namespace E_Food.Tests
{
    [TestFixture]
    public class UsuarioControllerTests
    {
        private Mock<IUnidadTrabajo> _unidadTrabajoMock;
        private Mock<IUserStore<IdentityUser>> _userStoreMock;
        private UsuarioController _controller;
        private Mock<IUserClaimsPrincipalFactory<IdentityUser>> _userClaimsPrincipalFactoryMock;
        private Mock<ITempDataDictionary> _tempDataMock;
        private Mock<ApplicationDbContext> _dbContextMock;

        [SetUp]
        public void Setup()
        {
            _unidadTrabajoMock = new Mock<IUnidadTrabajo>();
            _userStoreMock = new Mock<IUserStore<IdentityUser>>();
            _userClaimsPrincipalFactoryMock = new Mock<IUserClaimsPrincipalFactory<IdentityUser>>();
            _tempDataMock = new Mock<ITempDataDictionary>();
            _dbContextMock = new Mock<ApplicationDbContext>(new DbContextOptions<ApplicationDbContext>());

            _controller = new UsuarioController(_unidadTrabajoMock.Object, _dbContextMock.Object)
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
        public void Index_ReturnsViewResult()
        {
            // Act
            var result = _controller.Index();

            // Assert
            Assert.IsInstanceOf<ViewResult>(result);
        }

        [Test]
        public async Task Edit_WithNullId_ReturnsNotFoundResult()
        {
            // Arrange
            string userId = null;

            // Act
            var result = await _controller.Edit(userId);

            // Assert
            Assert.IsInstanceOf<NotFoundResult>(result);
        }

        [Test]
        public async Task Edit_WithValidModel_ReturnsRedirectToActionResult()
        {
            // Arrange
            var usuarioMock = new Usuario { Id = "validUserId", /* Otros campos necesarios */ };

            // Act
            var result = await _controller.Edit(usuarioMock);

            // Assert
            Assert.IsInstanceOf<RedirectToActionResult>(result);
            _unidadTrabajoMock.Verify(u => u.Usuario.Actualizar(usuarioMock), Times.Once);
            _unidadTrabajoMock.Verify(u => u.Guardar(), Times.Once);

            // Verifica que User.Identity no sea null
            Assert.IsNotNull(_controller.User.Identity);

            // Verifica que User.Identity.Name no sea null
            Assert.IsNotNull(_controller.User.Identity.Name);
        }


    }
}
