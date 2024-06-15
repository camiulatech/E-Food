using EFood.AccesoDatos.Data;
using EFood.AccesoDatos.Repositorio.IRepositorio;
using EFood.Modelos;


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
