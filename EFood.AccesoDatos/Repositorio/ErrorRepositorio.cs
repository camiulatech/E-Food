using EFood.AccesoDatos.Data;
using EFood.AccesoDatos.Repositorio.IRepositorio;
using EFood.Modelos;
using Microsoft.EntityFrameworkCore;

namespace EFood.AccesoDatos.Repositorio
{
    public class ErrorRepositorio : Repositorio<Error>, IErrorRepositorio
    {
        private readonly ApplicationDbContext _db;

        public ErrorRepositorio(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public async Task RegistrarError(string mensaje, int NumeroError)
        {
            var error = new Error
            {
                Fecha = DateTime.Now,
                Hora = DateTime.Now.ToString("HH:mm:ss"),
                Mensaje = mensaje,
                NumeroError = NumeroError
            };

            _db.Errors.Add(error);
            await _db.SaveChangesAsync();
        }

        public async Task<IEnumerable<Error>> ObtenerPorFecha(DateTime fecha)
        {
            return await _db.Errors.Where(e => e.Fecha.Date == fecha.Date).ToListAsync();
        }
    }
}
