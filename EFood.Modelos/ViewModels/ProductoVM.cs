using Microsoft.AspNetCore.Mvc.Rendering;

namespace EFood.Modelos.ViewModels
{
    public class ProductoVM
    {
        public Producto Producto { get; set; }

        public IEnumerable<SelectListItem> LineaComidaLista { get; set; }
        public IEnumerable<Producto> Productos { get; set; }

        public int? LineaComidaSeleccionadaId { get; set; }


    }
}
