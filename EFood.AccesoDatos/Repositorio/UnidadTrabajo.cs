using EFood.AccesoDatos.Data;
using EFood.AccesoDatos.Repositorio.IRepositorio;

namespace EFood.AccesoDatos.Repositorio
{
    public class UnidadTrabajo : IUnidadTrabajo
    {

        private readonly ApplicationDbContext _db;
        public ILineaComidaRepositorio LineaComida { get; private set; }
        public ITarjetaRepositorio Tarjeta { get; private set; }
        public IProductoRepositorio Producto{ get; private set; }
        public ITiquetesDescuentoRepositorio TiqueteDescuento { get; private set; }
        public IProcesadorPagoRepositorio ProcesadorPago { get; private set; }
        public IUsuarioRepositorio Usuario { get; private set; }
        public IErrorRepositorio Error {  get; private set; }
        public IBitacoraRepositorio Bitacora { get; private set; }
        public ITipoPrecioRepositorio TipoPrecio { get; private set; }
        public IPedidoRepositorio Pedido { get; private set; }


        public UnidadTrabajo(ApplicationDbContext db) {
            _db = db;
            LineaComida = new LineaComidaRepositorio(_db);

            Tarjeta = new TarjetaRepositorio(_db);

            TiqueteDescuento = new TiqueteDescuentoRepositorio(_db);

            Producto = new ProductoRepositorio(_db);

            ProcesadorPago = new ProcesadorPagoRepositorio(_db);

            Usuario = new UsuarioRepositorio(_db);

            Error = new ErrorRepositorio(_db);

            Bitacora = new BitacoraRepositorio(_db);

            TipoPrecio = new TipoPrecioRepositorio(_db);

            Pedido = new PedidoRepositorio(_db);
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
