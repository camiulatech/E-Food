using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace EFood.Modelos
{
    public class TipoPrecio
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Descripcion es Requerida")]
        [MaxLength(100, ErrorMessage = "Descripcion debe ser Maximo 100 caracteres")]
        public string Descripcion { get; set; }

        [Required(ErrorMessage = "Cambio es Requerido")]
        public decimal Cambio { get; set; }

        [NotMapped]
        public decimal? Monto { get; set; }

        [NotMapped]
        public int? ProductoId { get; set; }

        public List<Producto> Productos { get; set; }
    }
}
