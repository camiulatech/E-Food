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
    public class UsuarioRepositorio : Repositorio<Usuario>, IUsuarioRepositorio
    {

        private readonly ApplicationDbContext _db;

        public UsuarioRepositorio(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Actualizar(Usuario usuario)
        {
            var usuarioBD = _db.Usuarios.FirstOrDefault(c => c.Id == usuario.Id);
            if (usuarioBD != null)
            {
                usuarioBD.Estado = usuario.Estado;
                usuarioBD.Rol = usuario.Rol;

                _db.SaveChanges();
            }
        }

        public async Task<Usuario> ObtenerPorIdAsync(string id)
        {
            return await _db.Usuarios.FirstOrDefaultAsync(u => u.Id == id);
        }
    }
}
