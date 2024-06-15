using EFood.Modelos;

namespace EFood.AccesoDatos.Repositorio.IRepositorio
{
    public interface ITarjetaRepositorio : IRepositorio<Tarjeta>
    {
        void Actualizar(Tarjeta tarjeta);

    }
}
