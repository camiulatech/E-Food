using EFood.AccesoDatos.Repositorio.IRepositorio;
using EFood.Modelos;
using EFood.Utilidades;
using Microsoft.AspNetCore.Mvc;

namespace E_Food.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProcesadorPagoController : Controller
    {
        
        private readonly IUnidadTrabajo _unidadTrabajo;

        public ProcesadorPagoController(IUnidadTrabajo unidadTrabajo)
        {
            _unidadTrabajo = unidadTrabajo;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Upsert(int? id)
        {
            ProcesadorPago procesadorPago = new ProcesadorPago();

            if (id == null)
            {
                //Crear nuevo Procesador
                procesadorPago.Estado = true;
                return View(procesadorPago);
            }
            //Editar procesador
            procesadorPago = await _unidadTrabajo.ProcesadorPago.Obtener(id.GetValueOrDefault());
            if (procesadorPago == null)
            {
                return NotFound();
            }
            return View(procesadorPago);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upsert(ProcesadorPago procesadorPago)
        {

            if (ModelState.IsValid)
            {
                if (procesadorPago.Id == 0)
                {
                    await _unidadTrabajo.ProcesadorPago.Agregar(procesadorPago);
                    TempData[DS.Exitosa] = "Procesador de pago creado exitosamente";
                }
                else
                {
                    _unidadTrabajo.ProcesadorPago.Actualizar(procesadorPago);
                    TempData[DS.Exitosa] = "Procesador de pago actualizado exitosamente";
                }
                await _unidadTrabajo.Guardar();
                return RedirectToAction(nameof(Index));
            }
            TempData[DS.Error] = "Error al guardar Procesador de pago";
            return View(procesadorPago);
        }


        #region API

        [HttpGet]
        public async Task<IActionResult> getAll()
        {
            var all = await _unidadTrabajo.ProcesadorPago.ObtenerTodos();
            return Json(new { data = all });
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {

            var procesadorPagoDB = await _unidadTrabajo.ProcesadorPago.Obtener(id);
            if (procesadorPagoDB == null)
            {
                return Json(new { success = false, message = "Error al borrar el Procesador de pago" });
            }
            _unidadTrabajo.ProcesadorPago.Remover(procesadorPagoDB);
            await _unidadTrabajo.Guardar();
            return Json(new { success = true, message = "Procesador de pago borrado correctamente" });
        }


        [ActionName("NameValidate")]
        public async Task<IActionResult> NameValidate(string procesador, int id = 0)
        {
            bool valor = false;
            var lista = await _unidadTrabajo.ProcesadorPago.ObtenerTodos();
            if (id == 0)
            {
                valor = lista.Any(p => p.Procesador.ToLower().Trim() == procesador.ToLower().Trim());
            }
            else
            {
                valor = lista.Any(p => p.Procesador.ToLower().Trim() == procesador.ToLower().Trim() && p.Id != id);
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
