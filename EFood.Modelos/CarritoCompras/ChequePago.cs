using System.ComponentModel.DataAnnotations;


namespace EFood.Modelos.CarritoCompras
{
    public class ChequePago
    {
        [Required(ErrorMessage = "Numero de Cheque Requerido")]
        public int NumeroCheque { get; set; }

        [Required(ErrorMessage = "Cuenta es Requerido")]
        [MaxLength(22, ErrorMessage = "La cuenta debe tener 22 caracteres!")]
        public string Cuenta { get; set; }
    }
}
