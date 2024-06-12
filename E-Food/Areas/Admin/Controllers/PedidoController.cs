using EFood.AccesoDatos.Repositorio.IRepositorio;
using EFood.Modelos;
using EFood.Utilidades;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace E_Food.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = DS.Rol_Admin + "," + DS.Rol_Consulta)]
    public class PedidoController : Controller
    {

        private readonly IUnidadTrabajo _unidadTrabajo;

        public PedidoController(IUnidadTrabajo unidadTrabajo)
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
            var todos = await _unidadTrabajo.Pedido.ObtenerTodos(incluirPropiedades:"Productos");
            foreach (var pedido in todos)
            {
                foreach (var producto in pedido.Productos)
                {
                    producto.Pedidos = null;
                }
                pedido.TiqueteDescuento = await _unidadTrabajo.TiqueteDescuento.ObtenerPrimero(t => t.Id == pedido.TiqueteDescuentoId.GetValueOrDefault());
                pedido.ProcesadorPago = await _unidadTrabajo.ProcesadorPago.ObtenerPrimero(p => p.Id == pedido.ProcesadorPagoId);
                if (pedido.TiqueteDescuento == null)
                {
                    pedido.TiqueteDescuento = new TiqueteDescuento();
                    pedido.TiqueteDescuento.Codigo = "NA";
                }
            }
            return Json(new { data = todos });
        }


        [HttpGet]
        public async Task<IActionResult> ObtenerPorFecha(DateTime fecha)
        {
            var todos = await _unidadTrabajo.Pedido.ObtenerTodos(incluirPropiedades: "Productos");
            var registros = todos.Where(b => b.Fecha.Date == fecha.Date).ToList();
            foreach (var pedido in registros)
            {
                foreach (var producto in pedido.Productos)
                {
                    producto.Pedidos = null;
                }
                pedido.TiqueteDescuento = await _unidadTrabajo.TiqueteDescuento.ObtenerPrimero(t => t.Id == pedido.TiqueteDescuentoId.GetValueOrDefault());
                pedido.ProcesadorPago = await _unidadTrabajo.ProcesadorPago.ObtenerPrimero(p => p.Id == pedido.ProcesadorPagoId);
                if (pedido.TiqueteDescuento == null)
                {
                    pedido.TiqueteDescuento = new TiqueteDescuento();
                    pedido.TiqueteDescuento.Codigo = "NA";
                }
            }
            return Json(new { data = registros });
        }
        #endregion
    }
}
