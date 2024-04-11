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
        [Key]
        [Column(Order = 1)]
        [Required(ErrorMessage = "ID del producto es requerido")]
        public int IdProducto { get; set; }

        [Key]
        [Column(Order = 2)]
        [Required(ErrorMessage = "ID del tipo de precio es requerido")]
        public int IdTipoPrecio { get; set; }

        [ForeignKey("IdProducto")]
        public Producto Producto { get; set; }

        [ForeignKey("IdTipoPrecio")]
        public TipoPrecio TipoPrecio { get; set; }

        [Required(ErrorMessage = "Monto es requerido")]
        public float Monto { get; set; }
    }
}
