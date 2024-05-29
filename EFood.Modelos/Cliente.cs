using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFood.Modelos
{
    public class Cliente
    {
        [Required(ErrorMessage = "Nombre es Requerido")]
        [MaxLength(60, ErrorMessage = "Nombre debe ser Maximo 60 caracteres")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "Apellidos son Requeridos")]
        [MaxLength(60, ErrorMessage = "Apellidos deben ser Maximo 60 caracteres")]
        public string Apellidos { get; set; }

        [Required(ErrorMessage = "Teléfono es Requerido")]
        public string Telefono { get; set; }

        [Required(ErrorMessage = "Dirección de Envío es Requerida")]
        public string DireccionEnvio { get; set; }

        public string? TiqueteDescuento { get; set; }

    }
}
