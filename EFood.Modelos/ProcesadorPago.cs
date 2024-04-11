using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFood.Modelos
{
    public class ProcesadorPago
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre del procesador es requerido")]
        [MaxLength(100, ErrorMessage = "El nombre del procesador debe ser máximo de 100 caracteres")]
        public string Procesador { get; set; }

        [Required(ErrorMessage = "El nombre de la opción de pago es requerido")]
        [MaxLength(100, ErrorMessage = "El nombre de la opción de pago debe ser máximo de 100 caracteres")]
        public string NombreOpcionDePago { get; set; }

        [Required(ErrorMessage = "El tipo de procesador de pago es requerido")]
        public int TipoId { get; set; }

        [ForeignKey("TipoId")]
        public virtual TipoProcesadorPago IdTipo { get; set; }

        [Required(ErrorMessage = "El estado es requerido")]
        public EstadoProcesadorPago Estado { get; set; }

        [Required(ErrorMessage = "Es requerido especificar si requiere verificación")]
        public bool RequiereVerificacion { get; set; }

        [MaxLength(200, ErrorMessage = "El método debe ser máximo de 200 caracteres")]
        public string Metodo { get; set; }

        public enum EstadoProcesadorPago
        {
            Activo,
            Inactivo
        }
    }
}
