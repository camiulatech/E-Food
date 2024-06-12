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
        public void AgregarProductos(Pedido pedido, List<Producto> productos);

        public Task<IEnumerable<Bitacora>> ObtenerPorFecha(DateTime fecha);
    }
}
