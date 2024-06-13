using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using EFood.AccesoDatos.Repositorio.IRepositorio;
using EFood.Modelos.ViewModels;
using EFood.Modelos;
using EFood.Utilidades;
using EFood.Modelos.CarritoCompras;
using Newtonsoft.Json;

namespace EFoodCommerce.Areas.Commerce.Controllers
{
    [Area("Commerce")]
    public class CarritoCompraController : Controller
    {

        private const string SessionKeyCarrito = "Carrito";

        private readonly IUnidadTrabajo _unidadTrabajo;

        public CarritoCompraController(IUnidadTrabajo unidadTrabajo)
        {
            _unidadTrabajo = unidadTrabajo;
        }

        public IActionResult Index()
        {
            var carrito = ObtenerCarritoDeSesion();
            return View(carrito);
        }


        public async Task<IActionResult> ActualizarCantidad(int productoId, int tipoPrecioId, int cantidad)
        {
            var producto = await _unidadTrabajo.Producto.ObtenerPrimero(p => p.Id == productoId);
            var tipoPrecio = await _unidadTrabajo.TipoPrecio.ObtenerPrimero(t => t.Id == tipoPrecioId);
            if (producto == null || tipoPrecio == null)
            {
                return Json(new { success = false, message = "El producto no se pudo actualizar" });
            }
            var carrito = ObtenerCarritoDeSesion();
            carrito.ActualizarCantidad(producto, tipoPrecio, cantidad);
            GuardarCarritoEnSesion(carrito);
            return Json(new { success = true, message = "El producto se actualizó exitosamente!" });
        }

        private CarritoCompra ObtenerCarritoDeSesion()
        {
            var carritoJson = HttpContext.Session.GetString(SessionKeyCarrito);
            return carritoJson == null ? new CarritoCompra() : JsonConvert.DeserializeObject<CarritoCompra>(carritoJson);
        }

        private void GuardarCarritoEnSesion(CarritoCompra carrito)
        {
            var carritoJson = JsonConvert.SerializeObject(carrito);
            HttpContext.Session.SetString(SessionKeyCarrito, carritoJson);
        }

        public IActionResult Datos()
        {
            var cliente = new Cliente();
            return View(cliente);
        }

        public async Task<IActionResult> MetodoPago(Cliente cliente)
        {
            if (ModelState.IsValid)
            {
                ComprasVM comprasVM = new ComprasVM()
                {
                    Cliente = cliente,
                    CarritoCompra = ObtenerCarritoDeSesion()
                };
                if (cliente.TiqueteDescuento != null)
                {
                    comprasVM.TiqueteDescuento = await _unidadTrabajo.TiqueteDescuento.ObtenerPrimero(t => t.Codigo == cliente.TiqueteDescuento);
                }
                return View(comprasVM);

            }
            return RedirectToAction("Datos", cliente);
        }

        public IActionResult DatosPago()
        {
            var comprasVMJson = HttpContext.Session.GetString("ComprasVM");
            if (!string.IsNullOrEmpty(comprasVMJson))
            {
                var comprasVM = JsonConvert.DeserializeObject<ComprasVM>(comprasVMJson);
                return View(comprasVM);
            }
            return RedirectToAction("Index");
        }

        public IActionResult ConfirmarPago()
        {
            var comprasVMJson = HttpContext.Session.GetString("ComprasVM");
            if (!string.IsNullOrEmpty(comprasVMJson))
            {
                var comprasVM = JsonConvert.DeserializeObject<ComprasVM>(comprasVMJson);
                return View(comprasVM);
            }
            return RedirectToAction("Index");
        }

        public IActionResult LimpiarCarrito()
        {
            var carrito = ObtenerCarritoDeSesion();
            carrito.Limpiar();
            GuardarCarritoEnSesion(carrito);
            TempData[DS.Exitosa] = "Carrito limpiado exitosamente";
            return RedirectToAction("Index");
        }

        #region API

        [HttpPost]
        public async Task<IActionResult> Remover(int productoId, int tipoPrecioId)
        {
            var producto = await _unidadTrabajo.Producto.ObtenerPrimero(p => p.Id == productoId);
            var tipoPrecio = await _unidadTrabajo.TipoPrecio.ObtenerPrimero(t => t.Id == tipoPrecioId);
            if (producto == null || tipoPrecio == null)
            {
                TempData[DS.Error] = "El producto no se pudo remover";
                return Json(new { success = false, message = "El producto no se pudo remover" });
            }
            var carrito = ObtenerCarritoDeSesion();
            carrito.EliminarItem(producto, tipoPrecio);
            GuardarCarritoEnSesion(carrito);
            TempData[DS.Exitosa] = "El producto se removió exitosamente!";
            return Json(new { success = true, message = "El producto se removió exitosamente!" });
        }

        [HttpPost]
        public async Task<IActionResult> Datos(Cliente cliente)
        {
            if (ModelState.IsValid)
            {
                if (!string.IsNullOrEmpty(cliente.TiqueteDescuento))
                {
                    var tiquete = await _unidadTrabajo.TiqueteDescuento.ObtenerPrimero(t => t.Codigo == cliente.TiqueteDescuento);

                    if (tiquete == null || tiquete.Disponibles <= 0)
                    {
                        TempData[DS.Error] = "El tiquete no es válido";
                        return RedirectToAction("Datos", cliente);
                    }
                    else
                    {
                        return RedirectToAction("MetodoPago", cliente);
                    }
                }
                else
                {
                    return RedirectToAction("MetodoPago", cliente);
                }
            }
            return View(cliente);
        }

        [HttpPost]
        public async Task<IActionResult> MetodoPago([Bind("TipoProcesadorPago")] ComprasVM comprasVM, string CarritoCompraJson, string ClienteJson, string TiqueteDescuentoJson, string hiddenTipoProcesadorPago)
        {
            if (!string.IsNullOrEmpty(hiddenTipoProcesadorPago))
            {
                comprasVM.TipoProcesadorPago = Enum.Parse<TipoProcesadorPago>(hiddenTipoProcesadorPago);
            }

            if (!string.IsNullOrEmpty(CarritoCompraJson))
            {
                comprasVM.CarritoCompra = JsonConvert.DeserializeObject<CarritoCompra>(CarritoCompraJson);
            }

            if (!string.IsNullOrEmpty(ClienteJson))
            {
                comprasVM.Cliente = JsonConvert.DeserializeObject<Cliente>(ClienteJson);
            }

            if (!string.IsNullOrEmpty(TiqueteDescuentoJson))
            {
                comprasVM.TiqueteDescuento = JsonConvert.DeserializeObject<TiqueteDescuento>(TiqueteDescuentoJson);
            }

            if (ModelState.IsValid)
            {
                if (comprasVM.TipoProcesadorPago == TipoProcesadorPago.TarjetaDebitoCredito)
                {
                    comprasVM.TarjetaPago = new TarjetaPago();
                    comprasVM.ProcesadorPago = await _unidadTrabajo.ProcesadorPago.ObtenerPrimero(p => p.Tipo == comprasVM.TipoProcesadorPago && p.Estado == true, incluirPropiedades: "Tarjetas");
                    foreach (var x in comprasVM.ProcesadorPago.Tarjetas)
                    {
                        x.ProcesadorPagos = null;
                    }
                }
                else if (comprasVM.TipoProcesadorPago == TipoProcesadorPago.ChequeElectronico)
                {
                    comprasVM.ChequePago = new ChequePago();
                    comprasVM.ProcesadorPago = await _unidadTrabajo.ProcesadorPago.ObtenerPrimero(p => p.Tipo == comprasVM.TipoProcesadorPago && p.Estado == true);
                }
                else if (comprasVM.TipoProcesadorPago == TipoProcesadorPago.Efectivo)
                {
                    comprasVM.ProcesadorPago = await _unidadTrabajo.ProcesadorPago.ObtenerPrimero(p => p.Tipo == comprasVM.TipoProcesadorPago && p.Estado == true);
                }
                else
                {
                    return View(comprasVM);
                }
                HttpContext.Session.SetString("ComprasVM", JsonConvert.SerializeObject(comprasVM));
                return RedirectToAction("DatosPago");
            }
            return View(comprasVM);
        }

        [HttpPost]
        public async Task<IActionResult> DatosPago(ComprasVM compras, [Bind("TarjetaPago")] ComprasVM comprasVM, string CarritoCompraJson, string ClienteJson, string TiqueteDescuentoJson, string hiddenTipoProcesadorPago)
        {
            if (!string.IsNullOrEmpty(hiddenTipoProcesadorPago))
            {
                comprasVM.TipoProcesadorPago = Enum.Parse<TipoProcesadorPago>(hiddenTipoProcesadorPago);
            }

            if (!string.IsNullOrEmpty(CarritoCompraJson))
            {
                comprasVM.CarritoCompra = JsonConvert.DeserializeObject<CarritoCompra>(CarritoCompraJson);
            }

            if (!string.IsNullOrEmpty(ClienteJson))
            {
                comprasVM.Cliente = JsonConvert.DeserializeObject<Cliente>(ClienteJson);
            }

            if (!string.IsNullOrEmpty(TiqueteDescuentoJson))
            {
                comprasVM.TiqueteDescuento = JsonConvert.DeserializeObject<TiqueteDescuento>(TiqueteDescuentoJson);
            }

            if (comprasVM.TipoProcesadorPago == TipoProcesadorPago.TarjetaDebitoCredito)
            {
                comprasVM.ProcesadorPago = await _unidadTrabajo.ProcesadorPago.ObtenerPrimero(p => p.Tipo == comprasVM.TipoProcesadorPago && p.Estado == true, incluirPropiedades: "Tarjetas");
                foreach (var x in comprasVM.ProcesadorPago.Tarjetas)
                {
                    x.ProcesadorPagos = null;
                }
            }
            else if (comprasVM.TipoProcesadorPago == TipoProcesadorPago.ChequeElectronico)
            {
                comprasVM.ProcesadorPago = await _unidadTrabajo.ProcesadorPago.ObtenerPrimero(p => p.Tipo == comprasVM.TipoProcesadorPago && p.Estado == true);
                comprasVM.ChequePago = compras.ChequePago;
            }
            else if (comprasVM.TipoProcesadorPago == TipoProcesadorPago.Efectivo)
            {
                comprasVM.ProcesadorPago = await _unidadTrabajo.ProcesadorPago.ObtenerPrimero(p => p.Tipo == comprasVM.TipoProcesadorPago && p.Estado == true);
            }

            if (ModelState.IsValid)
            {
                if (comprasVM.TarjetaPago != null && (comprasVM.TarjetaPago.AñoExpiracion < DateTime.Now.Year || (comprasVM.TarjetaPago.AñoExpiracion == DateTime.Now.Year && comprasVM.TarjetaPago.MesExpiracion < DateTime.Now.Month)))
                {
                    TempData[DS.Error] = "La tarjeta de crédito ha expirado";
                    return View(comprasVM);
                }
                HttpContext.Session.SetString("ComprasVM", JsonConvert.SerializeObject(comprasVM));
                return RedirectToAction("ConfirmarPago");
            }
            return View(comprasVM);
        }

        [HttpPost]
        public async Task<IActionResult> ConfirmarPago(ComprasVM compras, [Bind("TarjetaPago")] ComprasVM comprasVM, string CarritoCompraJson, string ClienteJson, string TiqueteDescuentoJson, string hiddenTipoProcesadorPago)
        {
            if (!string.IsNullOrEmpty(hiddenTipoProcesadorPago))
            {
                comprasVM.TipoProcesadorPago = Enum.Parse<TipoProcesadorPago>(hiddenTipoProcesadorPago);
            }

            if (!string.IsNullOrEmpty(CarritoCompraJson))
            {
                comprasVM.CarritoCompra = JsonConvert.DeserializeObject<CarritoCompra>(CarritoCompraJson);
            }

            if (!string.IsNullOrEmpty(ClienteJson))
            {
                comprasVM.Cliente = JsonConvert.DeserializeObject<Cliente>(ClienteJson);
            }

            if (!string.IsNullOrEmpty(TiqueteDescuentoJson))
            {
                comprasVM.TiqueteDescuento = JsonConvert.DeserializeObject<TiqueteDescuento>(TiqueteDescuentoJson);
            }

            if (comprasVM.TipoProcesadorPago == TipoProcesadorPago.TarjetaDebitoCredito)
            {
                comprasVM.ProcesadorPago = await _unidadTrabajo.ProcesadorPago.ObtenerPrimero(p => p.Tipo == comprasVM.TipoProcesadorPago && p.Estado == true, incluirPropiedades: "Tarjetas");

            }
            else if (comprasVM.TipoProcesadorPago == TipoProcesadorPago.ChequeElectronico)
            {
                comprasVM.ProcesadorPago = await _unidadTrabajo.ProcesadorPago.ObtenerPrimero(p => p.Tipo == comprasVM.TipoProcesadorPago && p.Estado == true);
                comprasVM.ChequePago = compras.ChequePago;
            }
            else if (comprasVM.TipoProcesadorPago == TipoProcesadorPago.Efectivo)
            {
                comprasVM.ProcesadorPago = await _unidadTrabajo.ProcesadorPago.ObtenerPrimero(p => p.Tipo == comprasVM.TipoProcesadorPago && p.Estado == true);
            }

            if (ModelState.IsValid)
            {
                DateTime fechaPedido = DateTime.Now;
                var pedido = new Pedido()
                {
                    Fecha = fechaPedido,
                    Monto = comprasVM.CarritoCompra.ObtenerPrecio(),
                    Estado = EstadoPedido.Procesado,
                    TiqueteDescuentoId = comprasVM.TiqueteDescuento?.Id,
                    ProcesadorPagoId = comprasVM.ProcesadorPago.Id,
                };
                await _unidadTrabajo.Pedido.Agregar(pedido);
                await _unidadTrabajo.Guardar();

                var pedidoCompletado = await _unidadTrabajo.Pedido.ObtenerPrimero(p => p.Fecha == fechaPedido && p.Monto == comprasVM.CarritoCompra.ObtenerPrecio());
                _unidadTrabajo.Pedido.AgregarProductos(pedidoCompletado, comprasVM.CarritoCompra.ObtenerProductos());
                await _unidadTrabajo.Guardar();

                if (comprasVM.TiqueteDescuento != null)
                {
                    comprasVM.TiqueteDescuento.Disponibles--;
                    _unidadTrabajo.TiqueteDescuento.Actualizar(comprasVM.TiqueteDescuento);
                    await _unidadTrabajo.Guardar();
                }

                comprasVM.CarritoCompra.Limpiar();
                GuardarCarritoEnSesion(comprasVM.CarritoCompra);
                TempData[DS.Exitosa] = "Pedido realizado con éxito";

                return RedirectToAction("Index", "Home", new { area = "Inventario" });
            }
            return View(comprasVM);
        }

        #endregion
    }
}
