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
    public class TipoPrecioRepositorio : Repositorio<TipoPrecio>, ITipoPrecioRepositorio
    {

        private readonly ApplicationDbContext _db;

        public TipoPrecioRepositorio(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Actualizar(TipoPrecio tipoPrecio)
        {
            var tipoPrecioBD = _db.TipoPrecios.FirstOrDefault(c => c.Id == tipoPrecio.Id);
            if (tipoPrecio != null)
            {
                tipoPrecioBD.Descripcion = tipoPrecio.Descripcion;
                tipoPrecioBD.Cambio = tipoPrecio.Cambio;

                _db.SaveChanges();
            }
        }
    }
}
