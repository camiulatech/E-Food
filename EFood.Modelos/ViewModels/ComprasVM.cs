using EFood.Modelos.CarritoCompras;

namespace EFood.Modelos.ViewModels
{
    public class ComprasVM
    {
        public CarritoCompra CarritoCompra { get; set; }

        public TipoProcesadorPago TipoProcesadorPago { get; set; }

        public Cliente Cliente { get; set; }

        public TiqueteDescuento? TiqueteDescuento { get; set; }

        public ChequePago? ChequePago { get; set; }

        public TarjetaPago? TarjetaPago { get; set; }

        public ProcesadorPago? ProcesadorPago { get; set; }
    }
}
