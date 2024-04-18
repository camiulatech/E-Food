using EFood.AccesoDatos.Data;
using EFood.AccesoDatos.Repositorio.IRepositorio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFood.AccesoDatos.Repositorio
{
    public class UnidadTrabajo : IUnidadTrabajo
    {

        private readonly ApplicationDbContext _db;
        public ILineaComidaRepositorio LineaComida { get; private set; }
        public ITarjetaRepositorio Tarjeta { get; private set; }


        public ITiquetesDescuentoRepositorio TiqueteDescuento { get; private set; }

        public UnidadTrabajo(ApplicationDbContext db) {
            _db = db;
            LineaComida = new LineaComidaRepositorio(_db);

            Tarjeta = new TarjetaRepositorio(_db);

            TiqueteDescuento = new TiqueteDescuentoRepositorio(_db);
        }

        public void Dispose()
        {
            _db.Dispose();
        }

        public async Task Guardar()
        {
            await _db.SaveChangesAsync();
        }
    }
}
