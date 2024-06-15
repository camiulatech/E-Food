using System.ComponentModel.DataAnnotations;


namespace EFood.Modelos
{
    public class TiqueteDescuento
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Codigo es Requerido")]
        [MaxLength(10, ErrorMessage = "Codigo debe ser Maximo 10 caracteres")] 
        public string Codigo { get; set; }

        [Required(ErrorMessage = "Descripcion es Requerida")]
        [MaxLength(100, ErrorMessage = "Descripcion debe ser Maximo 100 caracteres")]
        public string Descripcion { get; set; }

        [Required(ErrorMessage = "Disponibles es Requerido")]
        public int Disponibles { get; set; }

        [Required(ErrorMessage = "Descuento es Requerido")]
        public int Descuento { get; set; }
    }
}
