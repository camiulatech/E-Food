using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFood.Modelos.CarritoCompras
{
    public class CarritoCompra
    {
        public List<ItemCarritoCompra> itemCarritoCompras { get; set; }

        public List<Producto> ObtenerProductos ()
        {
            // Obtener los productos en el carrito
            List<Producto> productos = new List<Producto>();
            if (itemCarritoCompras == null)
            {
                return productos;
            }
            foreach (var item in itemCarritoCompras)
            {
                productos.Add(item.Producto);
            }
            return productos;
        }

        public void AgregarItem(Producto producto, int cantidad, TipoPrecio tipoPrecio)
        {
            // Verificar si el producto ya está en el carrito
            if (itemCarritoCompras == null)
            {
                itemCarritoCompras = new List<ItemCarritoCompra>();
            }
            var itemExistente = itemCarritoCompras.FirstOrDefault(i => i.Producto.Id == producto.Id && i.TipoPrecio.Id == tipoPrecio.Id);

            if (itemExistente != null)
            {
                // Si el producto ya está en el carrito, actualizamos la cantidad
                itemExistente.Cantidad += cantidad;
            }
            else
            {
                // Si el producto no está en el carrito, lo añadimos como un nuevo elemento
                itemCarritoCompras.Add(new ItemCarritoCompra
                {
                    Producto = producto,
                    Cantidad = cantidad,
                    TipoPrecio = tipoPrecio
                });
            }
        }

        public void EliminarItem(Producto producto, TipoPrecio tipoPrecio)
        {
            // Eliminar el producto del carrito
            var item = itemCarritoCompras.FirstOrDefault(i => i.Producto.Id == producto.Id && i.TipoPrecio.Id == tipoPrecio.Id);
            if (item != null)
            {
                itemCarritoCompras.Remove(item);
            }
        }

        public void Limpiar()
        {
            // Limpiar el carrito
            itemCarritoCompras.Clear();
        }

        public void ActualizarCantidad(Producto producto, TipoPrecio tipoPrecio, int cantidad)
        {
            // Actualizar la cantidad de un producto en el carrito
            var item = itemCarritoCompras.FirstOrDefault(i => i.Producto.Id == producto.Id && i.TipoPrecio.Id == tipoPrecio.Id);
            if (item != null)
            {
                item.Cantidad = cantidad;
            }
        }

        public decimal ObtenerPrecio()
        {
            // Calcular el precio total sumando los precios de todos los productos en el carrito
            decimal precioTotal = 0;
            if (itemCarritoCompras == null)
            {
                return precioTotal;
            }
            foreach (var item in itemCarritoCompras)
            {
                precioTotal = Math.Round(precioTotal + ((item.Producto.Monto + (item.Producto.Monto * item.TipoPrecio.Cambio / 100)) * item.Cantidad), 2);
            }
            return precioTotal;
        }
    }
}
