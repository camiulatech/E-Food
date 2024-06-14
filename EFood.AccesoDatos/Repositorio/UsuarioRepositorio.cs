using EFood.AccesoDatos.Data;
using EFood.AccesoDatos.Repositorio.IRepositorio;
using EFood.Modelos;
using Microsoft.AspNetCore.Identity;

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
                var nuevoRol = _db.Roles.FirstOrDefault(r => r.Name == usuario.Rol);
                _db.UserRoles.RemoveRange(_db.UserRoles.Where(u => u.UserId == usuario.Id));
                _db.SaveChanges();

                usuarioBD.Estado = usuario.Estado;
                _db.UserRoles.Add(new IdentityUserRole<string> { UserId = usuario.Id, RoleId = nuevoRol.Id });
                _db.SaveChanges();
            }
        }

        public async Task<Usuario> ObtenerPorIdAsync(string id)
        {
            return await _db.Usuarios.FindAsync(id);  
        }
    }
}
