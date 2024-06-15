using EFood.Modelos;

namespace EFood.AccesoDatos.Repositorio.IRepositorio
{
    public interface ILineaComidaRepositorio : IRepositorio<LineaComida>
    {
        void Actualizar(LineaComida lineaComida);

    }
}
