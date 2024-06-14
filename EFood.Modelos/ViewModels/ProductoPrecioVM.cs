using Microsoft.AspNetCore.Mvc.Rendering;


namespace EFood.Modelos.ViewModels
{
    public class ProductoPrecioVM
    {
        public Producto Producto { get; set; }

        public TipoPrecio TipoPrecio { get; set; }

        public IEnumerable<SelectListItem> ListaTipoPrecios { get; set; }

    }
}
