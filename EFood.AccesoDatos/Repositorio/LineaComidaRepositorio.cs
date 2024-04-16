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
    public class LineaComidaRepositorio : Repositorio<LineaComidaRepositorio>, ILineaComidaRepositorio
    {

        private readonly ApplicationDbContext _db;

        public LineaComidaRepositorio(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(LineaComida lineaComida)
        {
            var lineaComidaBD = _db.LineaComidas.FirstOrDefault(c => c.Id == lineaComida.Id);
            if (lineaComida != null)
            {
                lineaComidaBD.Nombre = lineaComida.Nombre;

                _db.SaveChanges();
            }
        }
    }
}
