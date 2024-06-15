using EFood.Modelos;

namespace EFood.AccesoDatos.Repositorio.IRepositorio
{
    public interface ITipoPrecioRepositorio : IRepositorio<TipoPrecio>
    {
        void Actualizar(TipoPrecio tipoPrecio);

    }
}
