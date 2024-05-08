using Microsoft.AspNetCore.Mvc;
using EFood.AccesoDatos.Repositorio.IRepositorio;
using EFood.Modelos;
using EFood.Utilidades;
using Microsoft.AspNetCore.Authorization;
using EFood.Modelos.ViewModels;
using Microsoft.AspNetCore.Hosting;
using EFood.AccesoDatos.Repositorio;

namespace E_Food.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = DS.Rol_Admin + "," + DS.Rol_Mantemiento)]
    public class ProductoController : Controller
    {

        private readonly IUnidadTrabajo _unidadTrabajo;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ProductoController(IUnidadTrabajo unidadTrabajo, IWebHostEnvironment webHostEnvironment)
        {
            _unidadTrabajo = unidadTrabajo;
            _webHostEnvironment = webHostEnvironment;
        }


        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Upsert(int? id)
        {
            ProductoVM productoVM = new ProductoVM()
            {
                Producto = new Producto(),
                LineaComidaLista = _unidadTrabajo.Producto.ObtenerLineasComidasListaDesplegable("LineaComida")

            };
            if (id == null)
            {
                return View(productoVM);
            }

            productoVM.Producto = await _unidadTrabajo.Producto.Obtener(id.GetValueOrDefault());
            if (productoVM.Producto == null)
            {
                return NotFound();
            }
            return View(productoVM);
        }

        public async Task<IActionResult> Consultar()
        {
            var productos = await _unidadTrabajo.Producto.ObtenerTodos(); // Obtener la lista de productos (puedes ajustar este método según tu implementación)
            return View(productos);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upsert(ProductoVM productoVM)
        {

            if (ModelState.IsValid)
            {
                var usuarioNombre = User.Identity.Name;

                var archivos = HttpContext.Request.Form.Files;
                string webRootPath = _webHostEnvironment.WebRootPath;

                if (productoVM.Producto.Id == 0)
                {
                    string upload = webRootPath + DS.ImagenRuta;
                    string fileName = Guid.NewGuid().ToString();
                    string extension = Path.GetExtension(archivos[0].FileName);

                    using (var fileStream = new FileStream(Path.Combine(upload, fileName + extension), FileMode.Create))
                    {
                        archivos[0].CopyTo(fileStream);
                    }
                    productoVM.Producto.UbicacionImagen = fileName + extension;
                    await _unidadTrabajo.Producto.Agregar(productoVM.Producto);
                    await _unidadTrabajo.Guardar();

                    var idRegistro = productoVM.Producto.Id;
                    await _unidadTrabajo.Bitacora.RegistrarBitacora(usuarioNombre, idRegistro.ToString(), $"Se insertó el producto '{productoVM.Producto.Nombre}' con ID: {idRegistro}");

                }
                else
                {
                    // Actualizar
                    var objProducto = await _unidadTrabajo.Producto.ObtenerPrimero(p => p.Id == productoVM.Producto.Id, isTracking: false);
                    if (archivos.Count > 0) //Se carga una nueva imagen
                    {
                        string upload = webRootPath + DS.ImagenRuta;
                        string fileName = Guid.NewGuid().ToString();
                        string extension = Path.GetExtension(archivos[0].FileName);

                        //Borar la imagen anterior
                        var anteriorFile = Path.Combine(upload, objProducto.UbicacionImagen);
                        if (System.IO.File.Exists(anteriorFile))
                        {
                            System.IO.File.Delete(anteriorFile);
                        }
                        using (var fileStream = new FileStream(Path.Combine(upload, fileName + extension), FileMode.Create))
                        {
                            archivos[0].CopyTo(fileStream);
                        }
                        productoVM.Producto.UbicacionImagen = fileName + extension;

                    } //Caso no se carga imagen
                    else
                    {
                        productoVM.Producto.UbicacionImagen = objProducto.UbicacionImagen;
                    }
                    _unidadTrabajo.Producto.Actualizar(productoVM.Producto);
                    await _unidadTrabajo.Bitacora.RegistrarBitacora(usuarioNombre, productoVM.Producto.Id.ToString(), $"Se actualizó el producto '{productoVM.Producto.Nombre}' con ID: {productoVM.Producto.Id}");

                }
                TempData[DS.Exitosa] = "Transaccion exitosa!";
                await _unidadTrabajo.Guardar();
                return View("Index");
            } // If not Valid
            productoVM.LineaComidaLista = _unidadTrabajo.Producto.ObtenerLineasComidasListaDesplegable("LineaComida");
            return View(productoVM);
        }

        #region API

        [HttpGet]
        public async Task<IActionResult> ObtenerTodos()
        {
            var todos = await _unidadTrabajo.Producto.ObtenerTodos(incluirPropiedades: "LineaComida");
            return Json(new { data = todos });
        }

        [HttpPost]
        public async Task<IActionResult> Eliminar(int id)
        {
            var usuarioNombre = User.Identity.Name;

            var ProductoBD = await _unidadTrabajo.Producto.Obtener(id);
            if (ProductoBD == null)
            {
                return Json(new { success = false, message = "Error al borrar el Producto" });
            }
            // Remover imagen
            string upload = _webHostEnvironment.WebRootPath + DS.ImagenRuta;
            var anteriorFile = Path.Combine(upload, ProductoBD.UbicacionImagen);
            if (System.IO.File.Exists(anteriorFile))
            {
                System.IO.File.Delete(anteriorFile);
            }

            _unidadTrabajo.Producto.Remover(ProductoBD);
            await _unidadTrabajo.Guardar();

            await _unidadTrabajo.Bitacora.RegistrarBitacora(usuarioNombre, ProductoBD.Id.ToString(), $"Se eliminó el producto '{ProductoBD.Nombre}' con ID: {ProductoBD.Id}");
            return Json(new { success = true, message = "Producto borrado exitosamente" });
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
