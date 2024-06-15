using EFood.AccesoDatos.Data;
using EFood.AccesoDatos.Repositorio.IRepositorio;
using EFood.Modelos;

namespace EFood.AccesoDatos.Repositorio
{
    public class TiqueteDescuentoRepositorio : Repositorio<TiqueteDescuento>, ITiquetesDescuentoRepositorio
    {

        private readonly ApplicationDbContext _db;

        public TiqueteDescuentoRepositorio(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Actualizar(TiqueteDescuento tiqueteDescuento)
        {
            var tiqueteDescuentoBD = _db.TiqueteDescuentos.FirstOrDefault(c => c.Id == tiqueteDescuento.Id);
            if (tiqueteDescuento != null)
            {
                tiqueteDescuentoBD.Codigo = tiqueteDescuento.Codigo;
                tiqueteDescuentoBD.Descripcion = tiqueteDescuento.Descripcion;
                tiqueteDescuentoBD.Disponibles = tiqueteDescuento.Disponibles;
                tiqueteDescuentoBD.Descuento = tiqueteDescuento.Descuento;

                _db.SaveChanges();
            }
        }
    }
}
