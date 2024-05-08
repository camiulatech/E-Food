using EFood.Modelos;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EFood.AccesoDatos.Repositorio.IRepositorio
{
    public interface IErrorRepositorio : IRepositorio<Error>
    {
        Task RegistrarError(string mensaje, int numeroError);

        Task<IEnumerable<Error>> ObtenerPorFecha(DateTime fecha);
    }
}
