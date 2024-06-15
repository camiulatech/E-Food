using EFood.AccesoDatos.Data;
using EFood.AccesoDatos.Repositorio.IRepositorio;
using EFood.Modelos;


namespace EFood.AccesoDatos.Repositorio
{
    public class LineaComidaRepositorio : Repositorio<LineaComida>, ILineaComidaRepositorio
    {

        private readonly ApplicationDbContext _db;

        public LineaComidaRepositorio(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Actualizar(LineaComida lineaComida)
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
