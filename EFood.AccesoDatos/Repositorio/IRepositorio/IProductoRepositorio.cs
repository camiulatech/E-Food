using EFood.Modelos;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace EFood.AccesoDatos.Repositorio.IRepositorio
{
    public interface IProductoRepositorio : IRepositorio<Producto>
    {
        void Actualizar(Producto producto);
        Task<IEnumerable<Producto>> FiltrarPorLineaComida(int value);
        IEnumerable<SelectListItem> ObtenerLineasComidasListaDesplegable(string objeto);

        IEnumerable<SelectListItem> ObtenerTipoPreciosListaDesplegable(string objeto);

        void AgregarPrecio(Producto producto, TipoPrecio tipoPrecio);

        void RemoverPrecio(Producto producto, TipoPrecio tipoPrecio);
    }
}
