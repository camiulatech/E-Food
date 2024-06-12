using EFood.AccesoDatos.Data;
using EFood.AccesoDatos.Repositorio;
using EFood.AccesoDatos.Repositorio.IRepositorio;
using EFood.Modelos;
using EFood.Modelos.ViewModels;
using EFood.Utilidades;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;

namespace E_Food.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = DS.Rol_Admin + "," + DS.Rol_Mantemiento + "," + DS.Rol_Consulta)]
    public class ProductoPrecioController : Controller
    {

        private readonly IUnidadTrabajo _unidadTrabajo;

        public ProductoPrecioController(IUnidadTrabajo unidadTrabajo)
        {
            _unidadTrabajo = unidadTrabajo;
        }


        public IActionResult Index(int? id)
        {
            if (id != null)
            {
                ViewData["ProductoId"] = id;
            }
            return View();
        }

        //Es un get por defecto
        public async Task<IActionResult> Upsert(int? id)
        {
            ProductoPrecioVM productoPrecioVM = new ProductoPrecioVM()
            {
                Producto = new Producto(),
                TipoPrecio = new TipoPrecio(),
                ListaTipoPrecios = _unidadTrabajo.Producto.ObtenerTipoPreciosListaDesplegable("TipoPrecio"),

            };

            if (id == null)
            {
                return View(productoPrecioVM);
            }
            Producto productoExiste = await _unidadTrabajo.Producto.Obtener(id.GetValueOrDefault());
            if (productoExiste == null)
            {
                return NotFound();
            }
            productoPrecioVM.Producto = productoExiste;
            return View(productoPrecioVM);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upsert(ProductoPrecioVM productoPrecioVM)
        {
            if (productoPrecioVM.Producto != null && productoPrecioVM.TipoPrecio != null)
            {
                productoPrecioVM.Producto = await _unidadTrabajo.Producto.ObtenerPrimero(x => x.Id == productoPrecioVM.Producto.Id, incluirPropiedades: "TipoPrecios");
                productoPrecioVM.TipoPrecio = await _unidadTrabajo.TipoPrecio.Obtener(productoPrecioVM.TipoPrecio.Id);
                var usuarioNombre = User.Identity.Name;

                if (productoPrecioVM.Producto.TipoPrecios == null)
                {
                    productoPrecioVM.Producto.TipoPrecios = new List<TipoPrecio>();
                }

                if (productoPrecioVM.Producto.TipoPrecios.FirstOrDefault(tp => tp.Id == productoPrecioVM.TipoPrecio.Id) != null)
                {
                    TempData[DS.Error] = "El tipo de precio ya existe para este producto";
                    productoPrecioVM.ListaTipoPrecios = _unidadTrabajo.Producto.ObtenerTipoPreciosListaDesplegable("TipoPrecio");
                    return View(productoPrecioVM);  
                } 
                _unidadTrabajo.Producto.AgregarPrecio(productoPrecioVM.Producto, productoPrecioVM.TipoPrecio);
                await _unidadTrabajo.Guardar();

                var idRegistro = productoPrecioVM.Producto.Id;
                TempData[DS.Exitosa] = "Tipo de Precio creado exitosamente";

                // Registra en la bitácora
                await _unidadTrabajo.Bitacora.RegistrarBitacora(usuarioNombre, idRegistro.ToString(), $"Se agregó el tipo de precio '{productoPrecioVM.TipoPrecio.Descripcion}' al producto con ID: {idRegistro}");
                await _unidadTrabajo.Guardar();
                return Redirect("/Admin/ProductoPrecio/Index/"+productoPrecioVM.Producto.Id);
            }
            var mensajeError = TempData[DS.Error] = "Error al guardar el tipo de precio para este producto";
            await _unidadTrabajo.Error.RegistrarError(mensajeError.ToString(), 409);
            return View();
        }

        #region API

        [HttpGet]
        public async Task<IActionResult> ObtenerTodos(int? id)
        {
            var producto = await _unidadTrabajo.Producto.ObtenerPrimero(x => x.Id == id, incluirPropiedades: "TipoPrecios");
            if (producto != null && producto.TipoPrecios != null)
            {
                var todos = producto.TipoPrecios;
                for (int i = 0; i < todos.Count; i++)
                {
                    todos[i].Monto = producto.Monto + producto.Monto * (todos[i].Cambio / 100);
                    todos[i].Productos = null;
                    todos[i].ProductoId = producto.Id;
                }
                return Json(new { data = todos });
            } 
            else
            {
                return Json(new {} );
            }
        }

        [HttpPost]
        public async Task<IActionResult> Eliminar(string id)
        {
            var usuarioNombre = User.Identity.Name;
            string[] separar = id.Split(",");

            int idProducto = int.Parse(separar[0]);
            int idPrecio = int.Parse(separar[1]);

            var ProductoDB = await _unidadTrabajo.Producto.Obtener(idProducto);
            var TipoPrecioDB = await _unidadTrabajo.TipoPrecio.Obtener(idPrecio);
            var mensajeError = "Error al borrar el Tipo de Precio";
            if (ProductoDB == null || TipoPrecioDB == null)
            {
                await _unidadTrabajo.Error.RegistrarError(mensajeError.ToString(), 420);
                return Json(new { success = false, mensajeError });
            }
            _unidadTrabajo.Producto.RemoverPrecio(ProductoDB, TipoPrecioDB);
            await _unidadTrabajo.Guardar();

            await _unidadTrabajo.Bitacora.RegistrarBitacora(usuarioNombre, ProductoDB.Id.ToString(), $"Se eliminó el tipo de precio '{TipoPrecioDB.Descripcion}' en el producto con ID: {ProductoDB.Id}");

            return Json(new { success = true, message = "Precio borrado correctamente" });
        }


        #endregion


    }
}
