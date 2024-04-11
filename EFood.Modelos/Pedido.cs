using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFood.Modelos
{
    public class Pedido
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public DateTime Fecha { get; set; }

        [Required]
        public float Monto { get; set; }

        [Required]
        public EstadoPedido Estado { get; set; }

        public int? TiqueteDescuentoId { get; set; } // Permite null si no se aplica un descuento

        [ForeignKey("TiqueteDescuentoId")]
        public virtual TiqueteDescuento TiqueteDescuento { get; set; } // Propiedad de navegación

        [Required]
        public int ProcesadorPagoId { get; set; }

        [ForeignKey("ProcesadorPagoId")]
        public virtual ProcesadorPago ProcesadorPago { get; set; } // Propiedad de navegación
    }

    public enum EstadoPedido
    {
        Procesado,
        Cancelado,
        EnCurso
    }
}
