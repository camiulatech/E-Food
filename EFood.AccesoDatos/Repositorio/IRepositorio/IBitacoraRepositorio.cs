using EFood.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFood.AccesoDatos.Repositorio.IRepositorio
{
    public interface IBitacoraRepositorio : IRepositorio<Bitacora>
    {

        Task RegistrarBitacora(string usuario, int codigoRegistro, string descripcion);

        Task<IEnumerable<Bitacora>> ObtenerPorFecha(DateTime fecha);
    }
}
