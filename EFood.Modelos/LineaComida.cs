using System.ComponentModel.DataAnnotations;

namespace EFood.Modelos
{
    public class LineaComida
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Nombre es requerido")]
        [MaxLength(60, ErrorMessage = "Nombre debe ser Maximo 60 caracteres")]
        public string Nombre { get; set; }
    }
}
