using EFood.AccesoDatos.Repositorio.IRepositorio;
using EFood.Modelos;
using Microsoft.AspNetCore.Mvc;

namespace E_Food.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class BitacoraController : Controller
    {

        private readonly IUnidadTrabajo _unidadTrabajo;

        public BitacoraController(IUnidadTrabajo unidadTrabajo)
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
            var todos = await _unidadTrabajo.Bitacora.ObtenerTodos();
            return Json(new { data = todos });
        }


        [HttpGet]
        public async Task<IActionResult> ObtenerPorFecha(DateTime fecha)
        {
            var registros = await _unidadTrabajo.Bitacora.ObtenerPorFecha(fecha);
            return Json(new { data = registros });
        }
        #endregion
    }
}
