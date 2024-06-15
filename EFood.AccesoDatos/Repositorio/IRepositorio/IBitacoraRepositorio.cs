using EFood.Modelos;

namespace EFood.AccesoDatos.Repositorio.IRepositorio
{
    public interface IBitacoraRepositorio : IRepositorio<Bitacora>
    {

        Task RegistrarBitacora(string usuario, string codigoRegistro, string descripcion);

        Task<IEnumerable<Bitacora>> ObtenerPorFecha(DateTime fecha);
    }
}
