using Microsoft.AspNetCore.Mvc;
using EFood.AccesoDatos.Repositorio.IRepositorio;
using EFood.Modelos;
using EFood.Utilidades;
using Microsoft.AspNetCore.Authorization;

namespace E_Food.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductoController : Controller
    {

        private readonly IUnidadTrabajo _unidadTrabajo;

        public ProductoController(IUnidadTrabajo unidadTrabajo)
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
            Producto producto = new Producto();
            //LineaComidaLista = _unidadTrabajo.Producto.ObtenerTodosDropdownLista("LineaComida");

            if (id == null)
            {
                //Crear nueva Linea de Comida

                return View(producto);
            }
            //Actualizar Linea de Comida
            producto = await _unidadTrabajo.Producto.Obtener(id.GetValueOrDefault());
            if (producto == null)
            {
                return NotFound();
            }
            return View(producto);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upsert(Producto producto)
        {

            if (ModelState.IsValid)
            {
                if (producto.Id == 0)
                {
                    await _unidadTrabajo.Producto.Agregar(producto);
                    TempData[DS.Exitosa] = "Producto creado exitosamente";
                }
                else
                {
                    _unidadTrabajo.Producto.Actualizar(producto);
                    TempData[DS.Exitosa] = "Producto actualizado exitosamente";
                }
                await _unidadTrabajo.Guardar();
                return RedirectToAction(nameof(Index));
            }
            TempData[DS.Error] = "Error al guardar el Productos";
            return View(producto);
        }

        #region API

        [HttpGet]
        public async Task<IActionResult> ObtenerTodos()
        {
            var todos = await _unidadTrabajo.Producto.ObtenerTodos();
            return Json(new { data = todos });
        }

        [HttpPost]
        public async Task<IActionResult> Eliminar(int id) //Delete video
        {

            var ProductoBD = await _unidadTrabajo.Producto.Obtener(id);
            if (ProductoBD == null)
            {
                return Json(new { success = false, message = "Error al borrar el Producto" });
            }
            _unidadTrabajo.Producto.Remover(ProductoBD);
            await _unidadTrabajo.Guardar();
            return Json(new { success = true, message = "Producto borrado correctamente" });
        }

        [ActionName("ValidarNombre")]
        public async Task<IActionResult> ValidarNombre(string nombre, int id = 0)
        {
            bool value = false;
            var list = await _unidadTrabajo.Producto.ObtenerTodos();
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
