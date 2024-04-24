using EFood.AccesoDatos.Repositorio.IRepositorio;
using EFood.Modelos;
using EFood.Utilidades;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Drawing;

namespace E_Food.Areas.Admin.Controllers
{
    [Area("Admin")]
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
                //Crear nueva Tarjeta

                return View(tarjeta);
            }
            //Editar Tarjeta
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
                if (tarjeta.Id == 0)
                {
                    await _unidadTrabajo.Tarjeta.Agregar(tarjeta);
                    TempData[DS.Exitosa] = "Tarjeta creada exitosamente";
                }
                else
                {
                    _unidadTrabajo.Tarjeta.Actualizar(tarjeta);
                    TempData[DS.Exitosa] = "Tarjeta actualizada exitosamente";
                }
                await _unidadTrabajo.Guardar();
                return RedirectToAction(nameof(Index));
            }
            TempData[DS.Error] = "Error al guardar Tarjeta";
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

            var tarjetaBD = await _unidadTrabajo.Tarjeta.Obtener(id);
            if (tarjetaBD == null)
            {
                return Json(new { success = false, message = "Error al borrar la tarjeta" });
            }
            _unidadTrabajo.Tarjeta.Remover(tarjetaBD);
            await _unidadTrabajo.Guardar();
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
