using System.ComponentModel.DataAnnotations;


namespace EFood.Modelos
{
    public class Bitacora
    {

        [Key]
        public int Id { get; set; }

        public string Usuario { get; set; }

        [Required]
        public DateTime Fecha { get; set; }

        [Required]
        public string CodigoRegistro { get; set; }

        [Required]
        [MaxLength(250)]
        public string Descripcion { get; set; }
    }
}
