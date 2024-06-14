using EFood.AccesoDatos.Repositorio.IRepositorio;
using EFood.Modelos;
using EFood.Modelos.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace EFood.Areas.Inventario.Controllers
{
    [Area("Inventario")]

    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUnidadTrabajo _unidadTrabajo;

        public HomeController(ILogger<HomeController> logger, IUnidadTrabajo unidadTrabajo)
        {
            _logger = logger;
            _unidadTrabajo = unidadTrabajo;
        }

        public async Task<IActionResult> Index(int? idLineaComida)
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
            //IEnumerable<Producto> productoLista = await _unidadTrabajo.Producto.ObtenerTodos();
            //return View(productoLista);
        }

        public IActionResult Ayuda()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpGet]
        public async Task<IActionResult> FiltrarProductos(int idLineaComida)
        {
            if (idLineaComida == 0)
            {
                var productos = await _unidadTrabajo.Producto.ObtenerTodos(incluirPropiedades: "LineaComida");
                return PartialView("_ProductosParciales", productos);
            }
            var productosFiltrados = await _unidadTrabajo.Producto.FiltrarPorLineaComida(idLineaComida);
            return PartialView("_ProductosParciales", productosFiltrados);
        }
    }
}
