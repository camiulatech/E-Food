using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFood.Modelos
{
    public class PedidoProducto
    {
        [Key]
        [Column(Order = 1)]
        [Required(ErrorMessage = "ID del Pedido es requerido")]
        public int IdPedido { get; set; }

        [Key]
        [Column(Order = 2)]
        [Required(ErrorMessage = "ID del producto es requerido")]
        public int IdProducto { get; set; }

        [ForeignKey("IdPedido")]
        public Pedido Pedido { get; set; }

        [ForeignKey("IdProducto")]
        public Producto Producto { get; set; }
    }
}

