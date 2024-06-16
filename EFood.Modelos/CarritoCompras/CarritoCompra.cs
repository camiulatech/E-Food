namespace EFood.Modelos.CarritoCompras
{
    public class CarritoCompra
    {
        public List<ItemCarritoCompra> itemCarritoCompras { get; set; }

        public List<Producto> ObtenerProductos ()
        {
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
            if (itemCarritoCompras == null)
            {
                itemCarritoCompras = new List<ItemCarritoCompra>();
            }
            var itemExistente = itemCarritoCompras.FirstOrDefault(i => i.Producto.Id == producto.Id && i.TipoPrecio.Id == tipoPrecio.Id);

            if (itemExistente != null)
            {
                itemExistente.Cantidad += cantidad;
            }
            else
            {
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
            var item = itemCarritoCompras.FirstOrDefault(i => i.Producto.Id == producto.Id && i.TipoPrecio.Id == tipoPrecio.Id);
            if (item != null)
            {
                itemCarritoCompras.Remove(item);
            }
        }

        public void Limpiar()
        {
            itemCarritoCompras.Clear();
        }

        public void ActualizarCantidad(Producto producto, TipoPrecio tipoPrecio, int cantidad)
        {
            var item = itemCarritoCompras.FirstOrDefault(i => i.Producto.Id == producto.Id && i.TipoPrecio.Id == tipoPrecio.Id);
            if (item != null)
            {
                item.Cantidad = cantidad;
            }
        }

        public decimal ObtenerPrecio()
        {
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
