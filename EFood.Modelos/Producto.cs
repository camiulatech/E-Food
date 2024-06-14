using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EFood.Modelos
{
    public class Producto
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Nombre es requerido")]
        [MaxLength(60, ErrorMessage = "Nombre debe tener máximo 60 caracteres")]
        public string Nombre { get; set; }


        [Required(ErrorMessage = "ID de línea de comida es requerido")]
        public int IdLineaComida { get; set; }

        [ForeignKey("IdLineaComida")]
        public LineaComida LineaComida { get; set; }

        [Required(ErrorMessage = "Contenido es requerido")]
        public string Contenido { get; set; }

        [Required(ErrorMessage = "El monto es requerido")]
        public decimal Monto { get; set; }

        public string? UbicacionImagen { get; set; }

        public List<Pedido>? Pedidos { get; set; }

        public List<TipoPrecio>? TipoPrecios { get; set; }
    }
}
