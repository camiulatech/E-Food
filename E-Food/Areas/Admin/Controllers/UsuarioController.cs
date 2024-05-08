using EFood.AccesoDatos.Data;
using EFood.AccesoDatos.Repositorio.IRepositorio;
using EFood.Modelos;
using EFood.Utilidades;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace E_Food.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = DS.Rol_Admin + "," + DS.Rol_Seguridad)]
    public class UsuarioController : Controller
    {

        private readonly IUnidadTrabajo _unidadTrabajo;
        private readonly ApplicationDbContext _db;


        public UsuarioController(IUnidadTrabajo unidadTrabajo, ApplicationDbContext db)
        {
            _unidadTrabajo = unidadTrabajo;
            _db = db;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Usuario usuario = await _unidadTrabajo.Usuario.ObtenerPorIdAsync(id);

            if (usuario == null)
            {
                return NotFound();
            }
            var roleId = _db.UserRoles.FirstOrDefault(u => u.UserId == usuario.Id).RoleId;
            usuario.Rol = _db.Roles.FirstOrDefault(u => u.Id == roleId).Name;
            return View(usuario);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Usuario usuario)
        {
            var usuarioNombre = User.Identity.Name;

            _unidadTrabajo.Usuario.Actualizar(usuario);
            TempData[DS.Exitosa] = "Usuario actualizado exitosamente";
            await _unidadTrabajo.Guardar();

            // Convierte el ID del usuario de string a int
            int usuarioId = usuario.Id.GetHashCode();   //*******************

            await _unidadTrabajo.Bitacora.RegistrarBitacora(usuarioNombre, usuarioId, $"Se actualizó el usuario '{usuario.UserName}' con ID: {usuario.Id}");    //*******************

            return RedirectToAction(nameof(Index));
        }



        #region API

        [HttpGet]
        public async Task<IActionResult> ObtenerTodos()
        {
            var usuarioLista = await _unidadTrabajo.Usuario.ObtenerTodos();
            var userRole = await _db.UserRoles.ToListAsync();
            var roles = await _db.Roles.ToListAsync();

            foreach (var usuario in usuarioLista)
            {
                var roleId = userRole.FirstOrDefault(u => u.UserId == usuario.Id).RoleId;
                usuario.Rol = roles.FirstOrDefault(u => u.Id == roleId).Name;
            }
            return Json(new { data = usuarioLista });
        }

        #endregion
    }
}
