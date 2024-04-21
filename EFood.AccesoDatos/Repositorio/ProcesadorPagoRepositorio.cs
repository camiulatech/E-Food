using EFood.AccesoDatos.Data;
using EFood.AccesoDatos.Repositorio.IRepositorio;
using EFood.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

    }
}
