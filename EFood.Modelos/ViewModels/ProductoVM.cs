using EFood.Modelos.CarritoCompras;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFood.Modelos.ViewModels
{
    public class ProductoVM
    {
        public Producto Producto { get; set; }

        public IEnumerable<SelectListItem> LineaComidaLista { get; set; }
        public IEnumerable<Producto> Productos { get; set; } // Definir esta propiedad para almacenar los productos

        public int? LineaComidaSeleccionadaId { get; set; } // Nuevo campo para almacenar el ID de la línea de comida seleccionada


    }
}
