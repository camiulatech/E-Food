using EFood.AccesoDatos.Repositorio.IRepositorio;
using EFood.Modelos.CarritoCompras;
using Microsoft.AspNetCore.Mvc;

namespace EFoodCommerce.Areas.Commerce.Controllers
{
    [Area("Commerce")]
    public class ClienteController : Controller
    {
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

        [HttpPost]
        public async Task<IActionResult> ValidarCliente(Cliente cliente)
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
    }
}
