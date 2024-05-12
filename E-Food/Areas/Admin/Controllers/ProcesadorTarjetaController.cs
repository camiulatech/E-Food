using EFood.AccesoDatos.Data;
using EFood.AccesoDatos.Repositorio;
using EFood.AccesoDatos.Repositorio.IRepositorio;
using EFood.Modelos;
using EFood.Modelos.ViewModels;
using EFood.Utilidades;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;

namespace E_Food.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = DS.Rol_Admin + "," + DS.Rol_Mantemiento + "," + DS.Rol_Consulta)]
    public class ProcesadorTarjetaController : Controller
    {

        private readonly IUnidadTrabajo _unidadTrabajo;

        public ProcesadorTarjetaController(IUnidadTrabajo unidadTrabajo)
        {
            _unidadTrabajo = unidadTrabajo;
        }


        public async Task<IActionResult> Index(int? id)
        {
            var procesadorPago = await _unidadTrabajo.ProcesadorPago.ObtenerPrimero(x => x.Id == id && x.Tipo == TipoProcesadorPago.TarjetaDebitoCredito);
            if (id != null)
            {
                ViewData["ProductoId"] = id;
            }
            if (procesadorPago == null)
            {
                TempData[DS.Error] = "Para asignar tarjetas el procesador debe ser del tipo 2!";
                return Redirect("/Admin/ProcesadorPago/Index");
            }
            return View();
        }

        #region API

        [HttpGet]
        public async Task<IActionResult> ObtenerTodos(int? id)
        {
            var procesador = await _unidadTrabajo.ProcesadorPago.ObtenerPrimero(x => x.Id == id && x.Tipo == TipoProcesadorPago.TarjetaDebitoCredito, incluirPropiedades: "Tarjetas");
            if (procesador != null && procesador.Tarjetas != null)
            {
                var todos = procesador.Tarjetas;
                for (int i = 0; i < todos.Count; i++)
                {
                    todos[i].ProcesadorPagos = null;
                }

                return Json(new { data = todos });
            }
            else
            {
                return Json(new {} );
            }
        }

        [HttpGet]
        public async Task<IActionResult> ObtenerNoAsociados(int? id)
        {
            var procesador = await _unidadTrabajo.ProcesadorPago.ObtenerPrimero(x => x.Id == id && x.Tipo == TipoProcesadorPago.TarjetaDebitoCredito, incluirPropiedades: "Tarjetas");
            var tarjetasNoAsociadas = await _unidadTrabajo.Tarjeta.ObtenerTodos(t => !t.ProcesadorPagos.Contains(procesador), incluirPropiedades: "ProcesadorPagos");
            if (tarjetasNoAsociadas != null && procesador != null)
            {
                var todos = tarjetasNoAsociadas.ToList();
                for (int i = 0; i < todos.Count; i++)
                {
                    todos[i].ProcesadorPagos = null;
                }

                return Json(new { data = todos });
            }
            else
            {
                return Json(new { });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Upsert(string id)
        {
            string[] separar = id.Split(",");

            int idProcesador = int.Parse(separar[0]);
            int idTarjeta = int.Parse(separar[1]);

            var procesador = await _unidadTrabajo.ProcesadorPago.ObtenerPrimero(x => x.Id == idProcesador, incluirPropiedades: "Tarjetas");
            var tarjeta = await _unidadTrabajo.Tarjeta.Obtener(idTarjeta);
            if (procesador != null && tarjeta != null)
            {
                var usuarioNombre = User.Identity.Name;

                if (procesador.Tarjetas == null)
                {
                    procesador.Tarjetas = new List<Tarjeta>();
                }

                if (procesador.Tarjetas.FirstOrDefault(tp => tp.Id == tarjeta.Id) != null)
                {
                    return Json(new { success = false, message = "La tarjeta ya existe para este procesador" });
                }
                _unidadTrabajo.ProcesadorPago.AgregarTarjeta(procesador, tarjeta);
                await _unidadTrabajo.Guardar();

                var idRegistro = procesador.Id;

                // Registra en la bitácora
                await _unidadTrabajo.Bitacora.RegistrarBitacora(usuarioNombre, idRegistro.ToString(), $"Se agregó la Tarjeta '{tarjeta.Nombre}' al Procesado de Pago con ID: {idRegistro}");
                await _unidadTrabajo.Guardar();
                return Json(new { success = true, message = "Tarjeta agregada exitosamente" });
            }
            return Json(new { success = false, message = "Error al cargar el procesador de pago" });
        }

        [HttpPost]
        public async Task<IActionResult> Eliminar(string id)
        {
            var usuarioNombre = User.Identity.Name;
            string[] separar = id.Split(",");

            int idProcesador = int.Parse(separar[0]);
            int idTarjeta = int.Parse(separar[1]);

            var ProcesadorBD = await _unidadTrabajo.ProcesadorPago.Obtener(idProcesador);
            var TarjetaBD = await _unidadTrabajo.Tarjeta.Obtener(idTarjeta);
            if (ProcesadorBD == null || TarjetaBD == null)
            {
                return Json(new { success = false, message = "Error al quitar la Tarjeta" });
            }
            _unidadTrabajo.ProcesadorPago.RemoverTarjeta(ProcesadorBD, TarjetaBD);
            await _unidadTrabajo.Guardar();

            await _unidadTrabajo.Bitacora.RegistrarBitacora(usuarioNombre, ProcesadorBD.Id.ToString(), $"Se eliminó la Tarjeta '{TarjetaBD.Nombre}' en el Procesador de Pago con ID: {ProcesadorBD.Id}");

            return Json(new { success = true, message = "Tarjeta eliminada correctamente" });
        }


        #endregion


    }
}
