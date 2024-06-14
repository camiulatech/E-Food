using EFood.Modelos;

namespace EFood.AccesoDatos.Repositorio.IRepositorio
{
    public interface IPedidoRepositorio : IRepositorio<Pedido>
    {
        public void AgregarProductos(Pedido pedido, List<Producto> productos);
    }
}
