using EFood.Modelos;

namespace EFood.AccesoDatos.Repositorio.IRepositorio
{
    public interface IUsuarioRepositorio : IRepositorio<Usuario>
    {
        void Actualizar(Usuario usuario);
        Task<Usuario> ObtenerPorIdAsync(string id);

    }
}
