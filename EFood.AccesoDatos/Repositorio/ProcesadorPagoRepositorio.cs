using EFood.AccesoDatos.Data;
using EFood.AccesoDatos.Repositorio.IRepositorio;
using EFood.Modelos;
using Microsoft.EntityFrameworkCore;

namespace EFood.AccesoDatos.Repositorio
{
    public class ProcesadorPagoRepositorio : Repositorio<ProcesadorPago>, IProcesadorPagoRepositorio
    {
        private readonly ApplicationDbContext _db;

        public ProcesadorPagoRepositorio(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Actualizar(ProcesadorPago procesadorPago)
        {
            var procesadorPagoDB = _db.ProcesadorPagos.FirstOrDefault(p => p.Id == procesadorPago.Id);
            if (procesadorPagoDB != null)
            {
                procesadorPagoDB.Procesador = procesadorPago.Procesador;
                procesadorPagoDB.Tipo = procesadorPago.Tipo;
                procesadorPagoDB.RequiereVerificacion = procesadorPago.RequiereVerificacion;
                procesadorPagoDB.Metodo = procesadorPago.Metodo;
                procesadorPagoDB.Estado = procesadorPago.Estado;

                _db.SaveChanges();
            }
        }

        public void AgregarTarjeta(ProcesadorPago procesadorPago, Tarjeta tarjeta)
        {
            var procesadorBD = _db.ProcesadorPagos.FirstOrDefault(c => c.Id == procesadorPago.Id);
            procesadorBD.Tarjetas.Add(tarjeta);
            _db.SaveChanges();
        }

        public void RemoverTarjeta(ProcesadorPago procesadorPago, Tarjeta tarjeta)
        {
            var procesadorBD = _db.ProcesadorPagos.Include(p => p.Tarjetas).FirstOrDefault(c => c.Id == procesadorPago.Id);
            var viejoTipoPrecioBD = procesadorBD.Tarjetas.FirstOrDefault(c => c.Id == tarjeta.Id);
            procesadorBD.Tarjetas.Remove(viejoTipoPrecioBD);
            _db.SaveChanges();

        }
    }
}
