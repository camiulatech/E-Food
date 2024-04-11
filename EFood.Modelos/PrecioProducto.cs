using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFood.Modelos
{
    public class PrecioProducto
    {
        [Required(ErrorMessage = "ID del producto es requerido")]
        public int IdProducto { get; set; }

        [ForeignKey("IdProducto")]
        public Producto Producto { get; set; }

        [Required(ErrorMessage = "ID del tipo de precio es requerido")]
        public int IdTipoPrecio { get; set; }

        [ForeignKey("IdTipoPrecio")]
        public TipoPrecio TipoPrecio { get; set; }

        [Required(ErrorMessage = "Monto es requerido")]
        public float Monto { get; set; }
    }
}
