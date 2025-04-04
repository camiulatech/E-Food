﻿using System.ComponentModel.DataAnnotations;


namespace EFood.Modelos.CarritoCompras
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

        public string TiqueteDescuento { get; set; }

    }
}
