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
        //[Key]
        //public int Id { get; set; }

        //[Required(ErrorMessage = "Usuario es requerido")]
        //[MaxLength(45, ErrorMessage = "Usuario debe tener máximo 45 caracteres")]
        //public string usuario { get; set; }

        //[Required(ErrorMessage = "Contraseña es requerida")]
        //[MaxLength(45, ErrorMessage = "Contraseña debe tener máximo 45 caracteres")]
        //public string Contrasena { get; set; }

        //[Required(ErrorMessage = "Correo es requerido")]
        //[MaxLength(45, ErrorMessage = "Correo debe tener máximo 45 caracteres")]
        //public string Correo { get; set; }

        [Required(ErrorMessage = "Pregunta de solicitud es requerida")]
        [MaxLength(45, ErrorMessage = "Pregunta de solicitud debe tener máximo 45 caracteres")]
        public string PreguntaSolicitud { get; set; }

        [Required(ErrorMessage = "Respuesta de seguridad es requerida")]
        [MaxLength(45, ErrorMessage = "Respuesta de seguridad debe tener máximo 45 caracteres")]
        public string RespuestaSeguridad { get; set; }

        //[ForeignKey("Rol")]
        //[Required(ErrorMessage = "Id de rol es requerido")]
        //public int IdRol { get; set; }

        //public Rol Rol { get; set; }

        [Required(ErrorMessage = "Estado es requerido")]
        public bool Estado { get; set; }

        //[Required(ErrorMessage = "Rol es requerido")]
        //public RolUsuario Rol { get; set; }

        [NotMapped] //No se agrega a la tabla
        public string Rol {  get; set; }
    }

}
