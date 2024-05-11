using EFood.AccesoDatos.Data;
using EFood.AccesoDatos.Repositorio.IRepositorio;
using EFood.Modelos;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
            // Crear una nueva instancia de Error con el mensaje y la fecha actual
            var error = new Error
            {
                Fecha = DateTime.Now,
                Hora = DateTime.Now.ToString("HH:mm:ss"),
                Mensaje = mensaje,
                NumeroError = NumeroError
            };

            // Agregar el nuevo error al DbSet y guardar los cambios en la base de datos
            _db.Errors.Add(error);
            await _db.SaveChangesAsync();
        }

        public async Task<IEnumerable<Error>> ObtenerPorFecha(DateTime fecha)
        {
            return await _db.Errors.Where(e => e.Fecha.Date == fecha.Date).ToListAsync();
        }
    }
}
