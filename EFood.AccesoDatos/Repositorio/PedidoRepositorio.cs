using EFood.AccesoDatos.Data;
using EFood.AccesoDatos.Repositorio.IRepositorio;
using EFood.Modelos;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
