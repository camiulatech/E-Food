using EFood.AccesoDatos.Repositorio.IRepositorio;
using EFood.Modelos;
using EFood.Utilidades;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace E_Food.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class LineaComidaController : Controller
    {

        private readonly IUnidadTrabajo _unidadTrabajo;

        public LineaComidaController(IUnidadTrabajo unidadTrabajo)
        {
            _unidadTrabajo = unidadTrabajo;
        }


        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Upsert(int? id)
        {
            LineaComida lineaComida = new LineaComida();

            if (id == null)
            {
                //Crear nueva Linea de Comida

                return View(lineaComida);
            }
            //Actualizar Linea de Comida
            lineaComida = await _unidadTrabajo.LineaComida.Obtener(id.GetValueOrDefault());
            if (lineaComida == null)
            {
                return NotFound();
            }
            return View(lineaComida);

        }

    }
}
