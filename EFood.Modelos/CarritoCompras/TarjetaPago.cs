using System.ComponentModel.DataAnnotations;

namespace EFood.Modelos.CarritoCompras
{
    public class TarjetaPago
    {
        [Required(ErrorMessage = "Numero de Tarjeta es Requerido")]
        [MaxLength(18, ErrorMessage = "Número de Tarjeta no puede exceder 18 caracteres")]
        [MinLength(13, ErrorMessage = "Número de Tarjeta debe tener mínimo 13 caracteres")]
        public string NumeroTarjeta { get; set; }

        public string NumeroTarjetaFormateado
        {
            get
            {   if (!string.IsNullOrEmpty(NumeroTarjeta))
                    if (NumeroTarjeta.Length <= 4)
                    {
                        return NumeroTarjeta;
                    }
                    else
                    {
                        return new string('*', NumeroTarjeta.Length - 4) + NumeroTarjeta.Substring(NumeroTarjeta.Length - 4);
                    }
                return string.Empty;
            }
        }

        [Required(ErrorMessage = "Nombre del Titular es Requerido")]
        public string NombreTitular { get; set; }

        [Required(ErrorMessage = "Mes de Expiracion es Requerido")]
        public int MesExpiracion { get; set; }

        [Required(ErrorMessage = "Año de Expiracion es Requerido")]
        public int AñoExpiracion { get; set; }

        [Required(ErrorMessage = "Codigo de Seguridad es Requerido")]
        [MaxLength(4, ErrorMessage = "Codigo de Seguridad debe tener a lo mucho 4 caracteres")]
        [MinLength(3, ErrorMessage = "Codigo de Seguridad debe tener mínimo 3 caracteres")]
        public string CodigoSeguridad { get; set; }

        public string TipoTarjeta { get; set; }
    }
}
