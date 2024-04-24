using EFood.AccesoDatos.Data;
using EFood.AccesoDatos.Repositorio.IRepositorio;
using EFood.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace EFood.AccesoDatos.Repositorio
{
    public class TarjetaRepositorio : Repositorio<Tarjeta>, ITarjetaRepositorio
    {

        private readonly ApplicationDbContext _db;

        public TarjetaRepositorio(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Actualizar(Tarjeta tarjeta)
        {
            var tarjetaBD = _db.Tarjetas.FirstOrDefault(c => c.Id == tarjeta.Id);
            if (tarjetaBD != null)
            {
                tarjetaBD.Nombre = tarjeta.Nombre;

                _db.SaveChanges();
            }
        }

    }
}
