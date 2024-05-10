using EFood.AccesoDatos.Repositorio.IRepositorio;
using EFood.Modelos;
using EFood.AccesoDatos.Data;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

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
            var productoBD = _db.Productos.FirstOrDefault(c => c.Id == producto.Id);
            if (productoBD != null)
            {
                if (productoBD.UbicacionImagen != null)
                {
                    productoBD.UbicacionImagen = producto.UbicacionImagen;
                }
                productoBD.Nombre = producto.Nombre;
                productoBD.IdLineaComida = producto.IdLineaComida;
                productoBD.Contenido = producto.Contenido;
                productoBD.UbicacionImagen = producto.UbicacionImagen;
                _db.SaveChanges();
            }
        }

        public IEnumerable<SelectListItem> ObtenerLineasComidasListaDesplegable (string objeto) 
        {
            if (objeto == "LineaComida")
            {
                return _db.LineaComidas.Select(c => new SelectListItem()
                {
                    Text = c.Nombre,
                    Value = c.Id.ToString()
                });
            } else
            {
                return null;
            }
        }

        public async Task<IEnumerable<Producto>> FiltrarPorLineaComida(int idLineaComida)
        {
            return await _db.Productos
                .Include(p => p.LineaComida)
                .Where(p => p.IdLineaComida == idLineaComida)
                .ToListAsync();
        }
    }
}
