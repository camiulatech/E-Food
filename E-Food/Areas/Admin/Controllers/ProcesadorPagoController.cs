using EFood.AccesoDatos.Repositorio.IRepositorio;
using EFood.Modelos;
using EFood.Utilidades;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace E_Food.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = DS.Rol_Admin)]
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
                var usuarioNombre = User.Identity.Name;

                if (procesadorPago.Id == 0)
                {

                    await _unidadTrabajo.ProcesadorPago.Agregar(procesadorPago);
                    await _unidadTrabajo.Guardar();
                    TempData[DS.Exitosa] = "Procesador de pago creado exitosamente";

                    var idRegistro = procesadorPago.Id;
                    await _unidadTrabajo.Bitacora.RegistrarBitacora(usuarioNombre, idRegistro.ToString(), $"Se insertó el procesador de pago '{procesadorPago.Procesador}' con ID: {idRegistro}");
                }
                else
                {
                    _unidadTrabajo.ProcesadorPago.Actualizar(procesadorPago);
                    TempData[DS.Exitosa] = "Procesador de pago actualizado exitosamente";

                    await _unidadTrabajo.Bitacora.RegistrarBitacora(usuarioNombre, procesadorPago.Id.ToString(), $"Se actualizó el procesador de pago '{procesadorPago.Procesador}' con ID: {procesadorPago.Id}");

                }
                await _unidadTrabajo.Guardar();
                return RedirectToAction(nameof(Index));
            }
            TempData[DS.Error] = "Error al guardar Procesador de pago";
            return View(procesadorPago);
        }


        #region API

        [HttpGet]
        public async Task<IActionResult> ObtenerTodos()
        {
            var all = await _unidadTrabajo.ProcesadorPago.ObtenerTodos();
            return Json(new { data = all });
        }

        [HttpPost]
        public async Task<IActionResult> Eliminar(int id)
        {
            var usuarioNombre = User.Identity.Name;

            var procesadorPagoDB = await _unidadTrabajo.ProcesadorPago.Obtener(id);
            if (procesadorPagoDB == null)
            {
                return Json(new { success = false, message = "Error al borrar el Procesador de pago" });
            }
            _unidadTrabajo.ProcesadorPago.Remover(procesadorPagoDB);
            await _unidadTrabajo.Guardar();

            await _unidadTrabajo.Bitacora.RegistrarBitacora(usuarioNombre, procesadorPagoDB.Id.ToString(), $"Se eliminó el procesador de pago '{procesadorPagoDB.Procesador}' con ID: {procesadorPagoDB.Id}");

            return Json(new { success = true, message = "Procesador de pago borrado correctamente" });
        }


        [ActionName("ValidarNombre")]
        public async Task<IActionResult> ValidarNombre(string nombre, int id = 0)
        {
            bool value = false;
            var list = await _unidadTrabajo.ProcesadorPago.ObtenerTodos();
            if (id == 0)
            {
                value = list.Any(c => c.Procesador.ToLower().Trim() == nombre.ToLower().Trim());
            }
            else
            {
                value = list.Any(c => c.Procesador.ToLower().Trim() == nombre.ToLower().Trim() && c.Id != id);
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
