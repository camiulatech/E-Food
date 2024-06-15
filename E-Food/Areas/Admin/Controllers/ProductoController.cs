using Microsoft.AspNetCore.Mvc;
using EFood.AccesoDatos.Repositorio.IRepositorio;
using EFood.Modelos;
using EFood.Utilidades;
using Microsoft.AspNetCore.Authorization;
using EFood.Modelos.ViewModels;
using EFood.AccesoDatos.Servicio;

namespace E_Food.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = DS.Rol_Admin + "," + DS.Rol_Mantemiento)]
    public class ProductoController : Controller
    {

        private readonly IUnidadTrabajo _unidadTrabajo;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IServicioStorage _servicioStorage;

        public ProductoController(IUnidadTrabajo unidadTrabajo, IWebHostEnvironment webHostEnvironment, IServicioStorage servicioStorage)
        {
            _unidadTrabajo = unidadTrabajo;
            _webHostEnvironment = webHostEnvironment;
            _servicioStorage = servicioStorage;
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



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upsert(ProductoVM productoVM)
        {

            if (ModelState.IsValid && productoVM.Producto.Monto > 0)
            {

                var usuarioNombre = User.Identity.Name;

                var archivos = HttpContext.Request.Form.Files;

                string rutaImagenes = Path.Combine(_webHostEnvironment.ContentRootPath, "..", "Imagenes");
                if (productoVM.Producto.Id == 0)
                {
                    var fileName = Guid.NewGuid().ToString();
                    string extension = Path.GetExtension(archivos[0].FileName);
                    var filePath = "";

                    using (var stream = archivos[0].OpenReadStream())
                    {
                        var containerName = "productos";
                        var folderName = "imagenes";
                        filePath = await _servicioStorage.UploadImageAsync(stream, containerName, folderName, fileName+extension);
                    }

                    productoVM.Producto.UbicacionImagen = fileName + extension;
                    await _unidadTrabajo.Producto.Agregar(productoVM.Producto);
                    await _unidadTrabajo.Guardar();

                    var idRegistro = productoVM.Producto.Id;
                    await _unidadTrabajo.Bitacora.RegistrarBitacora(usuarioNombre, idRegistro.ToString(), $"Se insertó el producto '{productoVM.Producto.Nombre}' con ID: {idRegistro}");

                }
                else
                {
                    var objProducto = await _unidadTrabajo.Producto.ObtenerPrimero(p => p.Id == productoVM.Producto.Id, isTracking: false);
                    if (archivos.Count > 0) 
                    {
                        string fileName = Guid.NewGuid().ToString();
                        string extension = Path.GetExtension(archivos[0].FileName);
                        var filePath = "";

                        using (var stream = archivos[0].OpenReadStream())
                        {
                            var containerName = "productos";
                            var folderName = "imagenes";
                            filePath = await _servicioStorage.UploadImageAsync(stream, containerName, folderName, fileName + extension);
                        }

                        productoVM.Producto.UbicacionImagen = fileName + extension;

                    } 
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
            } 
            if (productoVM.Producto.Monto <= 0)
            {
                TempData[DS.Error] = "Verifica que el precio inicial sea un valor positivo";
                productoVM.LineaComidaLista = _unidadTrabajo.Producto.ObtenerLineasComidasListaDesplegable("LineaComida");

                return View(productoVM);
            }
            var mensajeError = "Error al crear el producto";
            TempData[DS.Error] = mensajeError;
            await _unidadTrabajo.Error.RegistrarError(mensajeError, 409);
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

        [HttpGet]
        public async Task<IActionResult> Consultar(int? idLineaComida)
        {
            var productoVM = new ProductoVM();

            productoVM.LineaComidaLista = _unidadTrabajo.Producto.ObtenerLineasComidasListaDesplegable("LineaComida");

            if (idLineaComida.HasValue)
            {
                productoVM.Productos = await _unidadTrabajo.Producto.FiltrarPorLineaComida(idLineaComida.Value);
            }
            else
            {
                productoVM.Productos = await _unidadTrabajo.Producto.ObtenerTodos(incluirPropiedades: "LineaComida");
            }
            productoVM.LineaComidaSeleccionadaId = idLineaComida;
            return View(productoVM);
        }

        [HttpPost]
        public async Task<IActionResult> Eliminar(int id)
        {
            var usuarioNombre = User.Identity.Name;

            var ProductoBD = await _unidadTrabajo.Producto.Obtener(id);
            var mensajeError = "Error al borrar producto";
            if (ProductoBD == null)
            {
                await _unidadTrabajo.Error.RegistrarError(mensajeError, 420);
                return Json(new { success = false, mensajeError });
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
