using EFood.Modelos;
using EFood.AccesoDatos.Repositorio.IRepositorio;
using EFood.Utilidades;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Moq;
using NUnit.Framework;
using System.Security.Claims;
using System.Threading.Tasks;
using E_Food.Areas.Admin.Controllers;
using EFood.AccesoDatos.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq;

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
        private Usuario _usuarioActualizado;

        [SetUp]
        public void Setup()
        {
            _unidadTrabajoMock = new Mock<IUnidadTrabajo>();
            _userStoreMock = new Mock<IUserStore<IdentityUser>>();
            _userClaimsPrincipalFactoryMock = new Mock<IUserClaimsPrincipalFactory<IdentityUser>>();
            _tempDataMock = new Mock<ITempDataDictionary>();
            _dbContextMock = new Mock<ApplicationDbContext>(new DbContextOptions<ApplicationDbContext>());

            _unidadTrabajoMock.Setup(u => u.Usuario.Actualizar(It.IsAny<Usuario>()))
                .Callback<Usuario>(u => _usuarioActualizado = u);
            _unidadTrabajoMock.Setup(u => u.Guardar()).Returns(Task.CompletedTask);
            _unidadTrabajoMock.Setup(u => u.Bitacora.RegistrarBitacora(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns(Task.CompletedTask);

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
            var usuarioMock = new Usuario { Id = "validUserId", PreguntaSolicitud = "Pregunta", RespuestaSeguridad = "Respuesta", Estado = true };

            // Act
            var result = await _controller.Edit(usuarioMock);

            // Assert
            Assert.IsInstanceOf<RedirectToActionResult>(result);
        }

        [Test]
        public async Task Edit_WithValidModel_CallsActualizar()
        {
            // Arrange
            var usuarioMock = new Usuario { Id = "validUserId", PreguntaSolicitud = "Pregunta", RespuestaSeguridad = "Respuesta", Estado = true };

            // Act
            await _controller.Edit(usuarioMock);

            // Assert
            _unidadTrabajoMock.Verify(u => u.Usuario.Actualizar(It.Is<Usuario>(u => u.Id == usuarioMock.Id)), Times.Once);
        }

        [Test]
        public async Task Edit_WithValidModel_CallsGuardar()
        {
            // Arrange
            var usuarioMock = new Usuario { Id = "validUserId", PreguntaSolicitud = "Pregunta", RespuestaSeguridad = "Respuesta", Estado = true };

            // Act
            await _controller.Edit(usuarioMock);

            // Assert
            _unidadTrabajoMock.Verify(u => u.Guardar(), Times.Once);
        }

        [Test]
        public async Task Edit_WithValidModel_CallsRegistrarBitacora()
        {
            // Arrange
            var usuarioMock = new Usuario { Id = "validUserId", PreguntaSolicitud = "Pregunta", RespuestaSeguridad = "Respuesta", Estado = true };

            // Act
            await _controller.Edit(usuarioMock);

            // Assert
            _unidadTrabajoMock.Verify(u => u.Bitacora.RegistrarBitacora(
                It.Is<string>(un => un == "testuser"),
                It.Is<string>(uid => uid == usuarioMock.Id),
                It.Is<string>(desc => desc.Contains($"Se actualizó el usuario '{usuarioMock.UserName}' con ID: {usuarioMock.Id}"))
            ), Times.Once);
        }

        [Test]
        public void Edit_WithValidModel_HasUserIdentity()
        {
            // Act
            var userIdentity = _controller.User.Identity;

            // Assert
            Assert.IsNotNull(userIdentity);
            Assert.IsNotNull(userIdentity.Name);
        }

        [Test]
        public async Task Edit_WithValidModel_UpdatesUsuarioCorrectly()
        {
            // Arrange
            var usuarioInicial = new Usuario
            {
                Id = "validUserId",
                PreguntaSolicitud = "PreguntaInicial",
                RespuestaSeguridad = "RespuestaInicial",
                Estado = false
            };

            var usuarioMock = new Usuario
            {
                Id = "validUserId",
                PreguntaSolicitud = "PreguntaActualizada",
                RespuestaSeguridad = "RespuestaActualizada",
                Estado = true
            };

            _unidadTrabajoMock.Setup(u => u.Usuario.ObtenerPorIdAsync(usuarioMock.Id))
                              .ReturnsAsync(usuarioInicial);

            // Act
            await _controller.Edit(usuarioMock);

            // Assert
            Assert.IsNotNull(_usuarioActualizado);
            Assert.AreEqual(usuarioMock.Id, _usuarioActualizado.Id);
            Assert.AreEqual(usuarioMock.PreguntaSolicitud, _usuarioActualizado.PreguntaSolicitud);
            Assert.AreEqual(usuarioMock.RespuestaSeguridad, _usuarioActualizado.RespuestaSeguridad);
            Assert.AreEqual(usuarioMock.Estado, _usuarioActualizado.Estado);

            // Verificar que los atributos han cambiado
            Assert.AreNotEqual(usuarioInicial.PreguntaSolicitud, _usuarioActualizado.PreguntaSolicitud);
            Assert.AreNotEqual(usuarioInicial.RespuestaSeguridad, _usuarioActualizado.RespuestaSeguridad);
            Assert.AreNotEqual(usuarioInicial.Estado, _usuarioActualizado.Estado);
        }

    }
}
