using EFood.Modelos;

namespace EFood.AccesoDatos.Repositorio.IRepositorio
{
    public interface ITiquetesDescuentoRepositorio : IRepositorio<TiqueteDescuento>
    {
        void Actualizar(TiqueteDescuento tiqueteDescuento);

    }
}
