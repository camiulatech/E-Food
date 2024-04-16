using EFood.AccesoDatos.Repositorio.IRepositorio;
using EFood.Modelos;
using EFood.Utilidades;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace E_Food.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class LineaComidaController : Controller
    {

        private readonly IUnidadTrabajo _unidadTrabajo;

        public LineaComidaController(IUnidadTrabajo unidadTrabajo)
        {
            _unidadTrabajo = unidadTrabajo;
        }


        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Upsert(int? id)
        {
            LineaComida lineaComida = new LineaComida();

            if (id == null)
            {
                //Crear nueva Linea de Comida

                return View(lineaComida);
            }
            //Actualizar Linea de Comida
            lineaComida = await _unidadTrabajo.LineaComida.Obtener(id.GetValueOrDefault());
            if (lineaComida == null)
            {
                return NotFound();
            }
            return View(lineaComida);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upsert(LineaComida lineaComida)
        {

            if (ModelState.IsValid)
            {
                if (lineaComida.Id == 0)
                {
                    await _unidadTrabajo.LineaComida.Agregar(lineaComida);
                    TempData[DS.Exitosa] = "Linea de Comida creada exitosamente";
                }
                else
                {
                    _unidadTrabajo.LineaComida.Update(lineaComida);
                    TempData[DS.Exitosa] = "Linea de Comida actualizada exitosamente";
                }
                await _unidadTrabajo.Guardar();
                return RedirectToAction(nameof(Index));
            }
            TempData[DS.Error] = "Error al guardar la Linea de Comida";
            return View(lineaComida);
        }

        #region API

        [HttpGet]
        public async Task<IActionResult> ObtenerTodos()
        {
            var todos = await _unidadTrabajo.LineaComida.ObtenerTodos();
            return Json(new { data = todos });
        }

        [HttpPost]
        public async Task<IActionResult> Eliminar(int id) //Delete video
        {

            var LineaComidaDB = await _unidadTrabajo.LineaComida.Obtener(id);
            if (LineaComidaDB == null)
            {
                return Json(new { success = false, message = "Error al borrar el Linea de Comida" });
            }
            _unidadTrabajo.LineaComida.Remover(LineaComidaDB);
            await _unidadTrabajo.Guardar();
            return Json(new { success = true, message = "Linea de Comida borrado correctamente" });
        }

        [ActionName("ValidarNombre")]
        public async Task<IActionResult> ValidarNombre(string nombre, int id = 0)
        {
            bool value = false;
            var list = await _unidadTrabajo.LineaComida.ObtenerTodos();
            if (id == 0)
            {
                value = list.Any(c => c.Nombre.ToLower().Trim() == nombre.ToLower().Trim());
            }
            else
            {
                value = list.Any(c => c.Nombre.ToLower().Trim() == nombre.ToLower().Trim() && c.Id != id);
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
