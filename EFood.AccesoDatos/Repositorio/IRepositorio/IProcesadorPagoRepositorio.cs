using EFood.Modelos;

namespace EFood.AccesoDatos.Repositorio.IRepositorio
{
    public interface IProcesadorPagoRepositorio : IRepositorio<ProcesadorPago>
    {
        void Actualizar(ProcesadorPago procesadorPago);

        void AgregarTarjeta(ProcesadorPago procesadorPago, Tarjeta tarjeta);

        void RemoverTarjeta(ProcesadorPago procesadorPago, Tarjeta tarjeta);
    }
}
