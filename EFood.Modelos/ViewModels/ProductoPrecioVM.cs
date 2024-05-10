using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFood.Modelos.ViewModels
{
    public class ProductoPrecioVM
    {
        public Producto Producto { get; set; }

        public TipoPrecio TipoPrecio { get; set; }

        public IEnumerable<SelectListItem> ListaTipoPrecios { get; set; }

    }
}
