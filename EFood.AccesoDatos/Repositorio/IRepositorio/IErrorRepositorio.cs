using EFood.Modelos;

namespace EFood.AccesoDatos.Repositorio.IRepositorio
{
    public interface IErrorRepositorio : IRepositorio<Error>
    {
        Task RegistrarError(string mensaje, int numeroError);

        Task<IEnumerable<Error>> ObtenerPorFecha(DateTime fecha);
    }
}
