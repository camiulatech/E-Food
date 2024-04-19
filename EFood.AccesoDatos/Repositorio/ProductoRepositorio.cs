using EFood.AccesoDatos.Repositorio.IRepositorio;
using EFood.Modelos;
using EFood.AccesoDatos.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFood.AccesoDatos.Repositorio
{
    public class ProductoRepositorio : Repositorio<Producto>, IProductoRepositorio
    {
        private readonly ApplicationDbContext _db;

        public ProductoRepositorio(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Actualizar(Producto producto)
        {
            var productoBD = _db.LineaComidas.FirstOrDefault(c => c.Id == producto.Id);
            if (productoBD != null)
            {
                productoBD.Nombre = producto.Nombre;
                _db.SaveChanges();
            }
        }
    }
}
