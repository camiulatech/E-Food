using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFood.Modelos
{
    public class TipoPrecio
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Descripcion es Requerida")]
        [MaxLength(100, ErrorMessage = "Descripcion debe ser Maximo 100 caracteres")]
        public string Descripcion { get; set; }

        [Required(ErrorMessage = "El monto es requerido")]
        public decimal Monto { get; set; }

        // Relación con TipoPrecioProducto
        public List<Producto> Productos { get; set; }
    }
}
