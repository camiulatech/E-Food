using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace EFood.Modelos
{
    public class Pedido
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public DateTime Fecha { get; set; }

        [Required]
        public decimal Monto { get; set; }

        [Required]
        public EstadoPedido Estado { get; set; }

        public int? TiqueteDescuentoId { get; set; } 

        [ForeignKey("TiqueteDescuentoId")]
        public virtual TiqueteDescuento TiqueteDescuento { get; set; }

        [Required]
        public int ProcesadorPagoId { get; set; }

        [ForeignKey("ProcesadorPagoId")]
        public virtual ProcesadorPago ProcesadorPago { get; set; }

        public List<Producto> Productos { get; set; }
    }

    public enum EstadoPedido
    {
        Procesado,
        Cancelado,
        EnCurso
    }
}
