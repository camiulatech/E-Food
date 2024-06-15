using EFood.AccesoDatos.Data;
using EFood.AccesoDatos.Repositorio.IRepositorio;
using EFood.Modelos;
using Microsoft.EntityFrameworkCore;

namespace EFood.AccesoDatos.Repositorio
{
    public class BitacoraRepositorio : Repositorio<Bitacora>, IBitacoraRepositorio
    {

        private readonly ApplicationDbContext _db;
        public BitacoraRepositorio(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public async Task RegistrarBitacora(string usuario, string codigoRegistro, string descripcion)
        {
            var bitacora = new Bitacora
            {
                Usuario = usuario,
                Fecha = DateTime.Now,
                CodigoRegistro = codigoRegistro,
                Descripcion = descripcion
            };

            _db.Bitacoras.Add(bitacora);
            _db.SaveChanges();
        }

        public async Task<IEnumerable<Bitacora>> ObtenerPorFecha(DateTime fecha)
        {
            return await _db.Bitacoras.Where(b => b.Fecha.Date == fecha.Date).ToListAsync();
        }
    }
}
