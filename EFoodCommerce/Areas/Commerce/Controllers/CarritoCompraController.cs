using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using EFood.AccesoDatos.Repositorio.IRepositorio;
using EFood.Modelos.ViewModels;
using EFood.Modelos;
using EFood.Utilidades;
using EFood.Modelos.CarritoCompras;
using Newtonsoft.Json;

namespace EFoodCommerce.Areas.Commerce.Controllers
{
    [Area("Commerce")]
    public class CarritoCompraController : Controller
    {

        private const string SessionKeyCarrito = "Carrito";

        private readonly IUnidadTrabajo _unidadTrabajo;

        public CarritoCompraController(IUnidadTrabajo unidadTrabajo)
        {
            _unidadTrabajo = unidadTrabajo;
        }

        public IActionResult Index()
        {
            var carrito = ObtenerCarritoDeSesion();
            return View(carrito);
        }


        public async Task<IActionResult> ActualizarCantidad(int productoId, int tipoPrecioId, int cantidad)
        {
            var producto = await _unidadTrabajo.Producto.ObtenerPrimero(p => p.Id == productoId);
            var tipoPrecio = await _unidadTrabajo.TipoPrecio.ObtenerPrimero(t => t.Id == tipoPrecioId);
            if (producto == null || tipoPrecio == null)
            {
                return Json(new { success = false, message = "El producto no se pudo actualizar" });
            }
            var carrito = ObtenerCarritoDeSesion();
            carrito.ActualizarCantidad(producto, tipoPrecio, cantidad);
            GuardarCarritoEnSesion(carrito);
            return Json(new { success = true, message = "El producto se actualizó exitosamente!" });
        }

        private CarritoCompra ObtenerCarritoDeSesion()
        {
            var carritoJson = HttpContext.Session.GetString(SessionKeyCarrito);
            return carritoJson == null ? new CarritoCompra() : JsonConvert.DeserializeObject<CarritoCompra>(carritoJson);
        }

        private void GuardarCarritoEnSesion(CarritoCompra carrito)
        {
            var carritoJson = JsonConvert.SerializeObject(carrito);
            HttpContext.Session.SetString(SessionKeyCarrito, carritoJson);
        }

        public IActionResult Datos()
        {
            var cliente = new Cliente();
            return View(cliente);
        }

        [HttpPost]
        public async Task<IActionResult> Datos(Cliente cliente)
        {
            if (ModelState.IsValid)
            {
                if (!string.IsNullOrEmpty(cliente.TiqueteDescuento))
                {
                    var tiquete = await _unidadTrabajo.TiqueteDescuento.ObtenerPrimero(t => t.Codigo == cliente.TiqueteDescuento);

                    if (tiquete == null || tiquete.Disponibles <= 0)
                    {
                        TempData[DS.Error] = "El tiquete no es válido";
                        return RedirectToAction("Datos", cliente);
                    }
                    else
                    {
                        return RedirectToAction("MetodoPago", cliente);
                    }
                }
                else
                {
                    return RedirectToAction("MetodoPago", cliente);
                }
            }
            return View(cliente);
        }

        public async Task<IActionResult> MetodoPago(Cliente cliente)
        {
            if (ModelState.IsValid)
            {
                ComprasVM comprasVM = new ComprasVM()
                {
                    Cliente = cliente,
                    CarritoCompra = ObtenerCarritoDeSesion()
                };
                if (cliente.TiqueteDescuento != null)
                {
                    comprasVM.TiqueteDescuento = await _unidadTrabajo.TiqueteDescuento.ObtenerPrimero(t => t.Codigo == cliente.TiqueteDescuento);
                }
                return View(comprasVM);

            }
            return RedirectToAction("Datos", cliente);
        }

        [HttpPost]
        public async Task<IActionResult> MetodoPago(ComprasVM comprasVM)
        {
            if (ModelState.IsValid)
            {
                return View(comprasVM);
            }
            return View(comprasVM);
        }

        #region API

        #endregion
    }
}
