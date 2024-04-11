using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFood.Modelos
{
    public class TarjetaProcesador
    {
        [Key]
        [Column(Order = 1)]
        [Required(ErrorMessage = "ID de tarjeta es requerido")]
        public int IdTarjeta { get; set; }

        [Key]
        [Column(Order = 2)]
        [Required(ErrorMessage = "ID del procesador de pago es requerido")]
        public int IdProcesadorPago { get; set; }

        [ForeignKey("IdTarjeta")]
        public Tarjeta Tarjeta { get; set; }

        [ForeignKey("IdProcesadorPago")]
        public ProcesadorPago ProcesadorPago { get; set; }
    }
}

