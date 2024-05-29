using EFood.AccesoDatos.Repositorio.IRepositorio;
using EFood.Modelos;
using Microsoft.AspNetCore.Mvc;

namespace EFoodCommerce.Areas.Commerce.Controllers
{
    public class ClienteController : Controller
    {
        private readonly IUnidadTrabajo _unidadTrabajo;

        public ClienteController(IUnidadTrabajo unidadTrabajo)
        {
            _unidadTrabajo = unidadTrabajo;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ValidarCliente(Cliente model)
        {
            if (ModelState.IsValid)
            {
                if (!string.IsNullOrEmpty(model.TiqueteDescuento))
                {
                    var tiquete = await _unidadTrabajo.TiqueteDescuento
                        .ObtenerPrimero(t => t.Descripcion == model.TiqueteDescuento);

                    if (tiquete == null || tiquete.Disponibles <= 0)
                    {
                        ModelState.AddModelError("TiqueteDescuento", "El tiquete de descuento no es válido o no está disponible.");
                    }
                }

                if (ModelState.IsValid)
                {
                    // Procesar el pedido, guardar cliente, etc.
                    return RedirectToAction("PedidoExitoso");
                }
            }

            return View("Index", model);
        }

        public IActionResult PedidoExitoso()
        {
            return View();
        }
    }
}
