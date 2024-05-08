using EFood.AccesoDatos.Data;
using EFood.AccesoDatos.Repositorio.IRepositorio;
using EFood.Modelos;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFood.AccesoDatos.Repositorio
{
    public class BitacoraRepositorio : Repositorio<Bitacora>, IBitacoraRepositorio
    {

        private readonly ApplicationDbContext _db;
        public BitacoraRepositorio(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public async Task RegistrarBitacora(string usuario, int codigoRegistro, string descripcion)
        {
            // Crear una nueva instancia de Bitacora con la información proporcionada
            var bitacora = new Bitacora
            {
                Usuario = usuario,
                Fecha = DateTime.Now, // Fecha actual
                CodigoRegistro = codigoRegistro,
                Descripcion = descripcion
            };

            // Agregar la nueva instancia de Bitacora al DbSet y guardar los cambios en la base de datos
            _db.Bitacoras.Add(bitacora);
            _db.SaveChanges();
        }

        public async Task<IEnumerable<Bitacora>> ObtenerPorFecha(DateTime fecha)
        {
            return await _db.Bitacoras.Where(b => b.Fecha.Date == fecha.Date).ToListAsync();
        }
    }
}
