using EFood.AccesoDatos.Repositorio.IRepositorio;
using EFood.Utilidades;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace E_Food.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = DS.Rol_Admin)]
    public class ErrorController : Controller
    {
        private readonly IUnidadTrabajo _unidadTrabajo;

        public ErrorController(IUnidadTrabajo unidadTrabajo)
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
            var todos = await _unidadTrabajo.Error.ObtenerTodos();
            return Json(new { data = todos });
        }

        [HttpGet]
        public async Task<IActionResult> ObtenerPorFecha(DateTime fecha)
        {
            var registros = await _unidadTrabajo.Error.ObtenerPorFecha(fecha);
            return Json(new { data = registros });
        }

        #endregion
    }
}
