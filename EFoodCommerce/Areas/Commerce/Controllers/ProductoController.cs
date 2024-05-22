using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using EFood.AccesoDatos.Repositorio.IRepositorio;
using EFood.Modelos.ViewModels;
using EFood.Modelos;
using EFood.Utilidades;


namespace EFoodCommerce.Areas.Commerce.Controllers
{
    [Area("Commerce")]
    public class ProductoController : Controller
    {

        private readonly IUnidadTrabajo _unidadTrabajo;

        public ProductoController(IUnidadTrabajo unidadTrabajo)
        {
            _unidadTrabajo = unidadTrabajo;
        }


        public IActionResult Index()
        {
            return View();
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

        #endregion
    }
}
