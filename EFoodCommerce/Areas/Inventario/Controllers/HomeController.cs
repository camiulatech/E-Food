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

        public async Task<IActionResult> Index()
        {
            var productoVM = new ProductoVM();

            // Obtener la lista de líneas de comida
            productoVM.LineaComidaLista = _unidadTrabajo.Producto.ObtenerLineasComidasListaDesplegable("LineaComida");
            productoVM.Productos = await _unidadTrabajo.Producto.ObtenerTodos(incluirPropiedades: "LineaComida");
            return View(productoVM);
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

        [HttpGet]
        public async Task<IActionResult> BuscarProductos(string query, int idLineaComida)
        {
            if (idLineaComida == 0)
            {
                if (string.IsNullOrEmpty(query))
                {
                    var productos = await _unidadTrabajo.Producto.ObtenerTodos(incluirPropiedades: "LineaComida");
                    return PartialView("_ProductosParciales", productos);
                }
                var productosFiltrados = await _unidadTrabajo.Producto.ObtenerTodos(x => x.Nombre.Contains(query), incluirPropiedades: "LineaComida");

                return PartialView("_ProductosParciales", productosFiltrados);
            }
            if (string.IsNullOrEmpty(query))
            {
                var productos = await _unidadTrabajo.Producto.FiltrarPorLineaComida(idLineaComida);
                return PartialView("_ProductosParciales", productos);
            }
            var productosFiltradosConLineaComida = await _unidadTrabajo.Producto.FiltrarPorLineaComida(idLineaComida);
            var productosBuscados = productosFiltradosConLineaComida.Where(p => p.Nombre.Contains(query, StringComparison.OrdinalIgnoreCase)).ToList();

            return PartialView("_ProductosParciales", productosBuscados);
        }

        [HttpGet]
        public async Task<IActionResult> ObtenerSugerencias(string term, int idLineaComida)
        {
            var sugerencias = await _unidadTrabajo.Producto.ObtenerSugerencias(term, idLineaComida);
            return Json(sugerencias);
        }
    }
}
