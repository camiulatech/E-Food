using EFood.AccesoDatos.Repositorio.IRepositorio;
using EFood.Modelos;
using EFood.Utilidades;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace E_Food.Areas.Admin.Controllers
{
    [Area("Admin")]
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

        //Es un get por defecto
        public async Task<IActionResult> Upsert(int? id)
        {
            TiqueteDescuento tiqueteDescuento = new TiqueteDescuento();

            if (id == null)
            {
                //Crear nueva Linea de Comida

                return View(tiqueteDescuento);
            }
            //Actualizar Linea de Comida
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

            if (ModelState.IsValid)
            {
                if (tiqueteDescuento.Id == 0)
                {
                    await _unidadTrabajo.TiqueteDescuento.Agregar(tiqueteDescuento);
                    TempData[DS.Exitosa] = "Tiquete de Descuento creado exitosamente";
                }
                else
                {
                    _unidadTrabajo.TiqueteDescuento.Actualizar(tiqueteDescuento);
                    TempData[DS.Exitosa] = "Tiquete de Descuento actualizado exitosamente";
                }
                await _unidadTrabajo.Guardar();
                return RedirectToAction(nameof(Index));
            }
            TempData[DS.Error] = "Error al guardar el Tiquete de Descuento";
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
        public async Task<IActionResult> Eliminar(int id) //Delete video
        {

            var TiqueteDescuentoBD = await _unidadTrabajo.TiqueteDescuento.Obtener(id);
            if (TiqueteDescuentoBD == null)
            {
                return Json(new { success = false, message = "Error al borrar el Tiquete de Descuento" });
            }
            _unidadTrabajo.TiqueteDescuento.Remover(TiqueteDescuentoBD);
            await _unidadTrabajo.Guardar();
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
