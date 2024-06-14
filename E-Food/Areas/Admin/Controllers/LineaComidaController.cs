using EFood.AccesoDatos.Repositorio.IRepositorio;
using EFood.Modelos;
using EFood.Utilidades;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace E_Food.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = DS.Rol_Admin + "," + DS.Rol_Mantemiento + "," + DS.Rol_Consulta)]
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
                return View(lineaComida);
            }
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
                var usuarioNombre = User.Identity.Name;

                if (lineaComida.Id == 0)
                {
                    await _unidadTrabajo.LineaComida.Agregar(lineaComida);
                    await _unidadTrabajo.Guardar();

                    var idRegistro = lineaComida.Id;

                    TempData[DS.Exitosa] = "Linea de Comida creada exitosamente";

                    await _unidadTrabajo.Bitacora.RegistrarBitacora(usuarioNombre, idRegistro.ToString(), $"Se insertó la línea de comida '{lineaComida.Nombre}' con ID: {idRegistro}");
                }
                else
                {
                    _unidadTrabajo.LineaComida.Actualizar(lineaComida);
                    TempData[DS.Exitosa] = "Linea de Comida actualizada exitosamente";

                    await _unidadTrabajo.Bitacora.RegistrarBitacora(usuarioNombre, lineaComida.Id.ToString(), $"Se actualizó la línea de comida '{lineaComida.Nombre}' con ID: {lineaComida.Id}");
                }
                await _unidadTrabajo.Guardar();
                return RedirectToAction(nameof(Index));
            }
            var mensajeError = TempData[DS.Error] = "Error al guardar la Linea de Comida";
            await _unidadTrabajo.Error.RegistrarError(mensajeError.ToString(), 409);
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
        public async Task<IActionResult> Eliminar(int id)
        {
            var usuarioNombre = User.Identity.Name;

            var LineaComidaDB = await _unidadTrabajo.LineaComida.Obtener(id);
            var mensajeError = "Error al borrar linea de comida";
            if (LineaComidaDB == null)
            {
                await _unidadTrabajo.Error.RegistrarError(mensajeError, 420);
                return Json(new { success = false, message = "Error al borrar el Linea de Comida" });
            }
            _unidadTrabajo.LineaComida.Remover(LineaComidaDB);
            await _unidadTrabajo.Guardar();

            await _unidadTrabajo.Bitacora.RegistrarBitacora(usuarioNombre, LineaComidaDB.Id.ToString(), $"Se eliminó la línea de comida '{LineaComidaDB.Nombre}' con ID: {LineaComidaDB.Id}");

            return Json(new { success = true, message = "Linea de Comida borrado correctamente" });
        }

        [ActionName("ValidarNombre")]
        public async Task<IActionResult> ValidarNombre(string nombre, int id = 0)
        {
            bool valor = false;
            var lista = await _unidadTrabajo.LineaComida.ObtenerTodos();
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
