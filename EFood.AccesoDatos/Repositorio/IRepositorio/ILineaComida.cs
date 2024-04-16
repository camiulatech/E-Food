using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFood.AccesoDatos.Repositorio.IRepositorio
{
    public interface ILineaComida : IRepositorio<LineaComida>
    {
        void Update(LineaComida lineaComida);

    }
}
