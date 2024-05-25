using EFood.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFood.AccesoDatos.Repositorio.IRepositorio
{
    public interface ICarritoCompraRepositorio
    {
        void AgregarItem(Producto producto, int cantidad,TipoPrecio tipoPrecio);

        void EliminarItem(Producto producto);

        void Limpiar();

        decimal ObtenerPrecio();
    }
}
