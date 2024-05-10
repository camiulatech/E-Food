using EFood.Modelos;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFood.AccesoDatos.Repositorio.IRepositorio
{
    public interface IProductoRepositorio : IRepositorio<Producto>
    {
        void Actualizar(Producto producto);

        IEnumerable<SelectListItem> ObtenerLineasComidasListaDesplegable(string objeto);

        IEnumerable<SelectListItem> ObtenerTipoPreciosListaDesplegable(string objeto);

        void AgregarPrecio(Producto producto, TipoPrecio tipoPrecio);

        void RemoverPrecio(Producto producto, TipoPrecio tipoPrecio);
    }
}
