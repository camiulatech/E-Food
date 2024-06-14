using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using EFood.AccesoDatos.Repositorio.IRepositorio;
using EFood.Modelos.ViewModels;
using EFood.Modelos;
using EFood.Utilidades;
using EFood.Modelos.CarritoCompras;
using Newtonsoft.Json;


namespace EFoodCommerce.Areas.Inventario.Controllers
{
    [Area("Inventario")]
    public class ProductoController : Controller
    {

        private readonly IUnidadTrabajo _unidadTrabajo;

        private const string SessionKeyCarrito = "Carrito";

        public ProductoController(IUnidadTrabajo unidadTrabajo)
        {
            _unidadTrabajo = unidadTrabajo;
        }


        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Agregar(int productoId, int cantidad, int tipoPrecioId)
        {
            var producto = await _unidadTrabajo.Producto.ObtenerPrimero(p => p.Id == productoId);
            var tipoPrecio = await _unidadTrabajo.TipoPrecio.ObtenerPrimero(t => t.Id == tipoPrecioId);

            if (producto == null || tipoPrecio == null)
            {
                TempData[DS.Error] = "Transaccion fallida";
                return RedirectToAction("Consultar"); ;
            }
            var carrito = ObtenerCarritoDeSesion();
            carrito.AgregarItem(producto, cantidad, tipoPrecio);
            GuardarCarritoEnSesion(carrito);

            TempData[DS.Exitosa] = "Transaccion exitosa!";
            HttpContext.Session.SetString("ContadorCarrito", carrito.itemCarritoCompras.Count.ToString());
            TempData[DS.Contador] = carrito.itemCarritoCompras.Count.ToString();
            return RedirectToAction("Index", "Home", new { area = "Inventario" });
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

            // Obtener la lista de líneas de comida
            productoVM.LineaComidaLista = _unidadTrabajo.Producto.ObtenerLineasComidasListaDesplegable("LineaComida");

            if (idLineaComida.HasValue)
            {
                // Si se proporciona un ID de línea de comida, filtrar los productos por esa línea
                productoVM.Productos = await _unidadTrabajo.Producto.FiltrarPorLineaComida(idLineaComida.Value);
            }
            else
            {
                // Si no se proporciona un ID de línea de comida, cargar todos los productos
                productoVM.Productos = await _unidadTrabajo.Producto.ObtenerTodos(incluirPropiedades: "LineaComida");
            }
            productoVM.LineaComidaSeleccionadaId = idLineaComida;
            return View(productoVM);
        }

        [HttpGet]
        public async Task<IActionResult> Detalle(int id)
        {
            var producto = await _unidadTrabajo.Producto.ObtenerPrimero(o => o.Id == id, incluirPropiedades: "LineaComida,TipoPrecios");
            if (producto == null)
            {
                return NotFound();
            }
            foreach( var precio in producto.TipoPrecios)
            {
                precio.Monto = Math.Round(producto.Monto + producto.Monto * (precio.Cambio / 100), 2);
            }
            return View(producto);
        }

        #endregion
    }
}
