using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFood.Modelos.CarritoCompras
{
    public class ItemCarritoCompra
    {
        public Producto Producto { get; set; }

        public int Cantidad { get; set; }   

        public TipoPrecio TipoPrecio { get; set; }
    }
}
