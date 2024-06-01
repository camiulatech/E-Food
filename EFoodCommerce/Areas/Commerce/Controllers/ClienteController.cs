using EFood.AccesoDatos.Repositorio.IRepositorio;
using EFood.Modelos.CarritoCompras;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace EFoodCommerce.Areas.Commerce.Controllers
{
    [Area("Commerce")]
    public class ClienteController : Controller
    {
        private const string SessionKeyCarrito = "Carrito";
        private readonly IUnidadTrabajo _unidadTrabajo;

        public ClienteController(IUnidadTrabajo unidadTrabajo)
        {
            _unidadTrabajo = unidadTrabajo;
        }

        public IActionResult Index()
        {
            var cliente = new Cliente();
            return View(cliente);
        }

        [HttpPost]
        public async Task<IActionResult> Index(Cliente cliente)
        {
            if (ModelState.IsValid)
            {
                if (!string.IsNullOrEmpty(cliente.TiqueteDescuento))
                {
                    var tiquete = await _unidadTrabajo.TiqueteDescuento
                        .ObtenerPrimero(t => t.Codigo == cliente.TiqueteDescuento);

                    if (tiquete == null || tiquete.Disponibles <= 0)
                    {
                        ModelState.AddModelError("TiqueteDescuento", "El tiquete de descuento no es válido o no está disponible.");
                    }


                }

                //if (ModelState.IsValid)
                //{
                //    // Procesar el pedido, guardar cliente, etc.
                //    return RedirectToAction("Index");
                //}
            }

            return View(cliente);
        }

        private CarritoCompra ObtenerCarritoDeSesion()
        {
            var carritoJson = HttpContext.Session.GetString(SessionKeyCarrito);
            return carritoJson == null ? new CarritoCompra() : JsonConvert.DeserializeObject<CarritoCompra>(carritoJson);
        }

        public async Task<IActionResult> MetodoPago(Cliente cliente){
            if (ModelState.IsValid)
            {
                var carrito = ObtenerCarritoDeSesion;
                var precioTotal = carrito.
            }

        }

    



}
