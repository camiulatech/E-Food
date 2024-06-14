using EFood.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFood.AccesoDatos.Repositorio.IRepositorio
{
    public interface IPedidoRepositorio : IRepositorio<Pedido>
    {
        void AgregarProductos(Pedido pedido, List<Producto> productos);

        void Actualizar(Pedido pedido);
    }
}
