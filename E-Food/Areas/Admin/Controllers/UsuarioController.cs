using EFood.AccesoDatos.Data;
using EFood.AccesoDatos.Repositorio.IRepositorio;
using EFood.Modelos;
using EFood.Utilidades;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace E_Food.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = DS.Rol_Admin)]
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

        //Es un get por defecto
        public async Task<IActionResult> Edit(int? id)
        {
            Usuario usuario = new Usuario();

            //Actualizar Linea de Comida
            usuario = await _unidadTrabajo.Usuario.Obtener(id.GetValueOrDefault());
            if (usuario == null)
            {
                return NotFound();
            }
            return View(usuario);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Usuario usuario)
        {

            if (ModelState.IsValid)
            {
                _unidadTrabajo.Usuario.Actualizar(usuario);
                TempData[DS.Exitosa] = "Usuario actualizado exitosamente";
                await _unidadTrabajo.Guardar();
                return RedirectToAction(nameof(Index));
            }
            TempData[DS.Error] = "Error al actualizar el Usuario";
            return View(usuario);
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


        [HttpPost]
        public async Task<IActionResult> BloquearDesbloquear([FromBody] string id)
        {
            var usuario = await _unidadTrabajo.Usuario.ObtenerPrimero(u => u.Id == id);
            if (usuario == null)
            {
                return Json(new { success = false, message = "Error de Usuario" });
            }
            if (usuario.LockoutEnd != null && usuario.LockoutEnd > DateTime.Now)
            {
                // Usuario Bloqueado
                usuario.LockoutEnd = DateTime.Now;
            }
            else
            {
                usuario.LockoutEnd = DateTime.Now.AddYears(1000);
            }
            await _unidadTrabajo.Guardar();
            return Json(new { success = true, message = "Operacion Exitosa" });

        }

        #endregion

    }
}
