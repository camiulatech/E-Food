using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFood.Modelos
{
    public class Rol
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Nombre es Requerido")]
        [MaxLength(45, ErrorMessage = "Descripcion debe ser Maximo 45 caracteres")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "Descripcion es Requerida")]
        [MaxLength(100, ErrorMessage = "Descripcion debe ser Maximo 100 caracteres")]
        public string Descripcion { get; set; }
    }
}
