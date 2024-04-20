using EFood.AccesoDatos.Repositorio.IRepositorio;
using EFood.Modelos;
using Microsoft.AspNetCore.Mvc;

namespace E_Food.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProcesadorPagoController : Controller
    {
        
        private readonly IUnidadTrabajo _unidadTrabajo;

        public ProcesadorPagoController(IUnidadTrabajo unidadTrabajo)
        {
            _unidadTrabajo = unidadTrabajo;
        }





        public IActionResult Index()
        {
            return View();
        }
    }
}
