
using System.ComponentModel.DataAnnotations;

namespace EFood.Modelos
{
    public class Tarjeta
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Nombre es Requerido")]
        [MaxLength(60, ErrorMessage = "Descripcion debe ser Maximo 60 caracteres")]
        public string Nombre { get; set; }

        public List<ProcesadorPago>? ProcesadorPagos { get; set; }
    }
}
