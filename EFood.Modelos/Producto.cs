using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFood.Modelos
{
    public class Producto
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Nombre es requerido")]
        [MaxLength(60, ErrorMessage = "Nombre debe tener máximo 60 caracteres")]
        public string Nombre { get; set; }


        [Required(ErrorMessage = "ID de línea de comida es requerido")]
        public int IdLineaComida { get; set; }

        [ForeignKey("IdLineaComida")]
        public LineaComida LineaComida { get; set; }

        [Required(ErrorMessage = "Contenido es requerido")]
        public string Contenido { get; set; }

        public string? UbicacionImagen { get; set; }

        //[Required(ErrorMessage = "Precio es requerido")]
        //public float Precio { get; set; }   //mejor si lo cambiamos a decimal

        //PARA CREAR TABLA INTERMEDIA PedidoProducto
        public List<Pedido>? Pedidos { get; set; } //poner que acepte nulos

        // Relación con TipoPrecioProducto
        public List<TipoPrecio>? TipoPrecios { get; set; } //poner que acepte nulos
    }
}
