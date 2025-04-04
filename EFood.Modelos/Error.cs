﻿using System.ComponentModel.DataAnnotations;


namespace EFood.Modelos
{
    public class Error 
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int NumeroError { get; set; }

        [Required]
        public DateTime Fecha { get; set; }

        [Required]
        [MaxLength(8)] 
        public string Hora { get; set; } 

        [Required]
        [MaxLength(250)]
        public string Mensaje { get; set; }
    }
}
