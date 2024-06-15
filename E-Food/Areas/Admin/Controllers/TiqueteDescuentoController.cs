using EFood.AccesoDatos.Repositorio.IRepositorio;
using EFood.Modelos;
using EFood.Utilidades;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace E_Food.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = DS.Rol_Admin + "," + DS.Rol_Mantemiento)]
    public class TiqueteDescuentoController : Controller
    {

        private readonly IUnidadTrabajo _unidadTrabajo;

        public TiqueteDescuentoController(IUnidadTrabajo unidadTrabajo)
        {
            _unidadTrabajo = unidadTrabajo;
        }


        public IActionResult Index()
        {
            return View();
        }


        public async Task<IActionResult> Upsert(int? id)
        {
            TiqueteDescuento tiqueteDescuento = new TiqueteDescuento();

            if (id == null)
            {
                return View(tiqueteDescuento);
            }
            tiqueteDescuento = await _unidadTrabajo.TiqueteDescuento.Obtener(id.GetValueOrDefault());
            if (tiqueteDescuento == null)
            {
                return NotFound();
            }
            return View(tiqueteDescuento);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upsert(TiqueteDescuento tiqueteDescuento)
        {

            if (ModelState.IsValid && tiqueteDescuento.Disponibles > 0 && tiqueteDescuento.Descuento >= 0 && tiqueteDescuento.Descuento <= 100)
            {
                var usuarioNombre = User.Identity.Name;

                if (tiqueteDescuento.Id == 0)
                {
                    await _unidadTrabajo.TiqueteDescuento.Agregar(tiqueteDescuento);
                    await _unidadTrabajo.Guardar();
                    TempData[DS.Exitosa] = "Tiquete de Descuento creado exitosamente";

                    var idRegistro = tiqueteDescuento.Id;
                    await _unidadTrabajo.Bitacora.RegistrarBitacora(usuarioNombre, idRegistro.ToString(), $"Se insertó el tiquete de descuento '{tiqueteDescuento.Descripcion}' con ID: {idRegistro}");
                }
                else
                {
                    _unidadTrabajo.TiqueteDescuento.Actualizar(tiqueteDescuento);
                    TempData[DS.Exitosa] = "Tiquete de Descuento actualizado exitosamente";

                    await _unidadTrabajo.Bitacora.RegistrarBitacora(usuarioNombre, tiqueteDescuento.Id.ToString(), $"Se actualizó el tiquete de descuento '{tiqueteDescuento.Descripcion}' con ID: {tiqueteDescuento.Id}");

                }
                await _unidadTrabajo.Guardar();
                return RedirectToAction(nameof(Index));
            }
            if (tiqueteDescuento.Disponibles <= 0)
            {
                TempData[DS.Error] = ("La cantidad de tiquetes disponibles debe ser mayor a 0");
                return View(tiqueteDescuento);
            }
            if (tiqueteDescuento.Descuento < 0 || tiqueteDescuento.Descuento > 100)
            {
                TempData[DS.Error] = ("El porcentaje de descuento debe ser mayor o igual a 0 y menor o igual a 100");
                return View(tiqueteDescuento);
            }
            var mensajeError = TempData[DS.Error] = "Error al guardar el Tiquete de Descuento";
            await _unidadTrabajo.Error.RegistrarError(mensajeError.ToString(), 409);
            return View(tiqueteDescuento);
        }

        #region API

        [HttpGet]
        public async Task<IActionResult> ObtenerTodos()
        {
            var todos = await _unidadTrabajo.TiqueteDescuento.ObtenerTodos();
            return Json(new { data = todos });
        }

        [HttpPost]
        public async Task<IActionResult> Eliminar(int id) 
        {
            var usuarioNombre = User.Identity.Name;

            var TiqueteDescuentoBD = await _unidadTrabajo.TiqueteDescuento.Obtener(id);
            var mensajeError = "Error al borrar Tiquete de Descuento";
            if (TiqueteDescuentoBD == null)
            {
                await _unidadTrabajo.Error.RegistrarError(mensajeError, 420);
                return Json(new { success = false, mensajeError});
            }
            _unidadTrabajo.TiqueteDescuento.Remover(TiqueteDescuentoBD);
            await _unidadTrabajo.Guardar();

            await _unidadTrabajo.Bitacora.RegistrarBitacora(usuarioNombre, TiqueteDescuentoBD.Id.ToString(), $"Se eliminó la tarjeta '{TiqueteDescuentoBD.Descripcion}' con ID: {TiqueteDescuentoBD.Id}");

            return Json(new { success = true, message = "Tiquete de Descuento borrado correctamente" });
        }

        [ActionName("ValidarNombre")]
        public async Task<IActionResult> ValidarNombre(string nombre, int id = 0)
        {
            bool value = false;
            var list = await _unidadTrabajo.TiqueteDescuento.ObtenerTodos();
            if (id == 0)
            {
                value = list.Any(c => c.Descripcion.ToLower().Trim() == nombre.ToLower().Trim());
            }
            else
            {
                value = list.Any(c => c.Descripcion.ToLower().Trim() == nombre.ToLower().Trim() && c.Id != id);
            }
            if (value)
            {
                return Json(new { data = true });
            }
            return Json(new { data = false });

        }

        #endregion

    }
}
