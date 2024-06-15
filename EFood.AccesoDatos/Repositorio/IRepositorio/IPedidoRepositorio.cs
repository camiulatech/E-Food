using EFood.Modelos;

namespace EFood.AccesoDatos.Repositorio.IRepositorio
{
    public interface IPedidoRepositorio : IRepositorio<Pedido>
    {
        void AgregarProductos(Pedido pedido, List<Producto> productos);

        void Actualizar(Pedido pedido);
    }
}
