using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFood.Modelos
{
    public class TipoProcesadorPago
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Nombre es requerido")]
        [MaxLength(50, ErrorMessage = "El nombre debe ser de máximo 50 caracteres")]
        public string Nombre { get; set; }
    }
}
