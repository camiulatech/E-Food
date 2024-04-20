using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFood.AccesoDatos.Repositorio.IRepositorio
{
    public interface IUnidadTrabajo : IDisposable
    {
        ILineaComidaRepositorio LineaComida { get; }

        ITarjetaRepositorio Tarjeta { get; }

        ITiquetesDescuentoRepositorio TiqueteDescuento { get; }

        IProductoRepositorio Producto { get; }

        Task Guardar();
    }
}
