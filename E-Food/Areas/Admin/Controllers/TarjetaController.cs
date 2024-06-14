using EFood.AccesoDatos.Repositorio.IRepositorio;
using EFood.Modelos;
using EFood.Utilidades;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace E_Food.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = DS.Rol_Admin)]
    public class TarjetaController : Controller
    {

        private readonly IUnidadTrabajo _unidadTrabajo;

        public TarjetaController(IUnidadTrabajo unidadTrabajo)
        {
            _unidadTrabajo = unidadTrabajo;
        }


        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Upsert(int? id)
        {
            Tarjeta tarjeta = new Tarjeta();

            if (id == null)
            {

                return View(tarjeta);
            }
            tarjeta = await _unidadTrabajo.Tarjeta.Obtener(id.GetValueOrDefault());
            if (tarjeta == null)
            {
                return NotFound();
            }
            return View(tarjeta);

        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upsert(Tarjeta tarjeta)
        {

            if (ModelState.IsValid)
            {
                var usuarioNombre = User.Identity.Name;

                if (tarjeta.Id == 0)
                {
                    await _unidadTrabajo.Tarjeta.Agregar(tarjeta);
                    await _unidadTrabajo.Guardar();
                    TempData[DS.Exitosa] = "Tarjeta creada exitosamente";

                    var idRegistro = tarjeta.Id;
                    await _unidadTrabajo.Bitacora.RegistrarBitacora(usuarioNombre, idRegistro.ToString(), $"Se insertó la tarjeta '{tarjeta.Nombre}' con ID: {idRegistro}");
                }
                else
                {
                    _unidadTrabajo.Tarjeta.Actualizar(tarjeta);
                    TempData[DS.Exitosa] = "Tarjeta actualizada exitosamente";

                    await _unidadTrabajo.Bitacora.RegistrarBitacora(usuarioNombre, tarjeta.Id.ToString(), $"Se actualizó la tarjeta '{tarjeta.Nombre}' con ID: {tarjeta.Id}");

                }
                await _unidadTrabajo.Guardar();
                return RedirectToAction(nameof(Index));
            }
            var mensajeError = TempData[DS.Error] = "Error al guardar Tarjeta";
            await _unidadTrabajo.Error.RegistrarError(mensajeError.ToString(), 409);
            return View(tarjeta);
        }



        #region API

        [HttpGet]
        public async Task<IActionResult> ObtenerTodos()
        {
            var all = await _unidadTrabajo.Tarjeta.ObtenerTodos();
            return Json(new { data = all });
        }

        [HttpPost]
        public async Task<IActionResult> Eliminar(int id)
        {
            var usuarioNombre = User.Identity.Name;

            var tarjetaBD = await _unidadTrabajo.Tarjeta.Obtener(id);
            var mensajeError = "Error al borrar tarjeta";
            if (tarjetaBD == null)
            {
                await _unidadTrabajo.Error.RegistrarError(mensajeError, 420);
                return Json(new { success = false, mensajeError });
            }
            _unidadTrabajo.Tarjeta.Remover(tarjetaBD);
            await _unidadTrabajo.Guardar();

            await _unidadTrabajo.Bitacora.RegistrarBitacora(usuarioNombre, tarjetaBD.Id.ToString(), $"Se eliminó la tarjeta '{tarjetaBD.Nombre}' con ID: {tarjetaBD.Id}");

            return Json(new { success = true, message = "Tarjeta borrada correctamente" });
        }

        [ActionName("ValidarNombre")]
        public async Task<IActionResult> ValidarNombre(string nombre, int id = 0)
        {
            bool valor = false;
            var lista = await _unidadTrabajo.Tarjeta.ObtenerTodos();
            if (id == 0)
            {
                valor = lista.Any(c => c.Nombre.ToLower().Trim() == nombre.ToLower().Trim());
            }
            else
            {
                valor = lista.Any(c => c.Nombre.ToLower().Trim() == nombre.ToLower().Trim() && c.Id != id);
            }
            if (valor)
            {
                return Json(new { data = true });
            }
            return Json(new { data = false });

        }



        #endregion


    }
}
