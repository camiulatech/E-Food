using EFood.Modelos.CarritoCompras;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFood.Modelos.ViewModels
{
    public class ComprasVM
    {
        public CarritoCompra CarritoCompra { get; set; }

        public TipoProcesadorPago TipoProcesadorPago { get; set; }

        public Cliente Cliente { get; set; }

        public TiqueteDescuento? TiqueteDescuento { get; set; }
    }
}
