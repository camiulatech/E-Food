using EFood.AccesoDatos.Repositorio.IRepositorio;
using EFood.Modelos;
using EFood.Modelos.CarritoCompras;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EFood.AccesoDatos.Repositorio
{
    public class CarritoCompraRepositorio : ICarritoCompraRepositorio
    {
        private CarritoCompra carrito;

        public CarritoCompraRepositorio()
        {
            carrito = new CarritoCompra();
            carrito.itemCarritoCompras = new List<ItemCarritoCompra>();
        }

        public void AgregarItem(Producto producto, int cantidad, TipoPrecio tipoPrecio)
        {
            // Verificar si el producto ya está en el carrito
            var itemExistente = carrito.itemCarritoCompras.FirstOrDefault(i => i.Producto.Id == producto.Id);

            if (itemExistente != null)
            {
                // Si el producto ya está en el carrito, actualizamos la cantidad
                itemExistente.Cantidad += cantidad;
            }
            else
            {
                // Si el producto no está en el carrito, lo añadimos como un nuevo elemento
                carrito.itemCarritoCompras.Add(new ItemCarritoCompra
                {
                    Producto = producto,
                    Cantidad = cantidad,
                    TipoPrecio = tipoPrecio
                });
            }
        }

        public void EliminarItem(Producto producto)
        {
            // Eliminar el producto del carrito
            var item = carrito.itemCarritoCompras.FirstOrDefault(i => i.Producto.Id == producto.Id);
            if (item != null)
            {
                carrito.itemCarritoCompras.Remove(item);
            }
        }

        public void Limpiar()
        {
            // Limpiar el carrito
            carrito.itemCarritoCompras.Clear();
        }

        public decimal ObtenerPrecio()
        {
            // Calcular el precio total sumando los precios de todos los productos en el carrito
            decimal precioTotal = 0;
            foreach (var item in carrito.itemCarritoCompras)
            {
                precioTotal = precioTotal + ((item.Producto.Monto + (item.Producto.Monto * item.TipoPrecio.Cambio))*item.Cantidad);
            }
            return precioTotal;
        }
    }
}
