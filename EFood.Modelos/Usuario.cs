using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace EFood.Modelos
{
    public class Usuario : IdentityUser
    {

        [Required(ErrorMessage = "Pregunta de solicitud es requerida")]
        [MaxLength(45, ErrorMessage = "Pregunta de solicitud debe tener máximo 45 caracteres")]
        public string PreguntaSolicitud { get; set; }

        [Required(ErrorMessage = "Respuesta de seguridad es requerida")]
        [MaxLength(45, ErrorMessage = "Respuesta de seguridad debe tener máximo 45 caracteres")]
        public string RespuestaSeguridad { get; set; }

        [Required(ErrorMessage = "Estado es requerido")]
        public bool Estado { get; set; }

        [NotMapped]
        public string Rol {  get; set; }
    }

}
