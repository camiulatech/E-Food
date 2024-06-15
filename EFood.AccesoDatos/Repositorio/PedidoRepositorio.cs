using EFood.AccesoDatos.Data;
using EFood.AccesoDatos.Repositorio.IRepositorio;
using EFood.Modelos;


namespace EFood.AccesoDatos.Repositorio
{
    public class PedidoRepositorio : Repositorio<Pedido>, IPedidoRepositorio
    {
        private readonly ApplicationDbContext _db;

        public PedidoRepositorio(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void AgregarProductos(Pedido pedido, List<Producto> productos)
        {
            var pedidoBD = _db.Pedidos.FirstOrDefault(c => c.Id == pedido.Id);
            if (pedidoBD.Productos == null)
            {
                pedidoBD.Productos = new List<Producto>();
            }
            foreach (var producto in productos)
            {
                pedidoBD.Productos.Add(producto);
            }
            _db.SaveChanges();
        }

        public void Actualizar(Pedido pedido)
        {
            var pedidoBD = _db.Pedidos.FirstOrDefault(c => c.Fecha == pedido.Fecha);
            if (pedidoBD != null)
            {
                pedidoBD.Fecha = pedido.Fecha;
                pedidoBD.Monto = pedido.Monto;
                pedidoBD.Estado = pedido.Estado;
                pedidoBD.TiqueteDescuentoId = pedido.TiqueteDescuentoId;
                pedidoBD.ProcesadorPagoId = pedido.ProcesadorPagoId;
                _db.SaveChanges();
            }
        }
    }
}
