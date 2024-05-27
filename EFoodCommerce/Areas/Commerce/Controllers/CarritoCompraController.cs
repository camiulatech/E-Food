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

        public IActionResult Index()
        {
            var carrito = ObtenerCarritoDeSesion();
            return View(carrito);
        }

        public IActionResult Remover(Producto producto)
        {
            var carrito = ObtenerCarritoDeSesion();
            carrito.EliminarItem(producto);
            GuardarCarritoEnSesion(carrito);
            return RedirectToAction("Index");
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

        //private Producto ObtenerProductoPorId(int productoId)
        //{
        //    // Implementa este método para obtener el producto de tu base de datos
        //    return new Producto { Id = productoId, Nombre = "Producto de ejemplo", Precio = 10.0m };
        //}


        #region API

        #endregion
    }
}
