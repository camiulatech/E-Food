using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFood.Modelos
{
    public class Tarjeta
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Nombre es Requerido")]
        [MaxLength(60, ErrorMessage = "Descripcion debe ser Maximo 60 caracteres")]
        public string Nombre { get; set; }

        //PARA CREAR TABLA INTERMEDIA TarjetaProcesador
        public List<ProcesadorPago>? ProcesadorPagos { get; set; }
    }
}
