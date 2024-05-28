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

        [HttpPost]
        public async Task<IActionResult> Remover(int productoId, int tipoPrecioId)
        {
            var producto = await _unidadTrabajo.Producto.ObtenerPrimero(p => p.Id == productoId);
            var tipoPrecio = await _unidadTrabajo.TipoPrecio.ObtenerPrimero(t => t.Id == tipoPrecioId);
            if (producto == null || tipoPrecio == null)
            {
                return Json(new { success = false, message = "El producto no se pudo remover" });
            }
            var carrito = ObtenerCarritoDeSesion();
            carrito.EliminarItem(producto, tipoPrecio);
            GuardarCarritoEnSesion(carrito);
            return Json(new { success = true, message = "El producto se removió exitosamente!" });
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


        #region API

        #endregion
    }
}
