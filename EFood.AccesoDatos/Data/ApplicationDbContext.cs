using EFood.Modelos;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace EFood.AccesoDatos.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<LineaComida> LineaComidas { get; set; }
        public DbSet<Pedido> Pedidos { get; set; }
        public DbSet<ProcesadorPago> ProcesadorPagos { get; set; }
        public DbSet<Producto> Productos { get; set; }
        public DbSet<Tarjeta> Tarjetas { get; set; }
        public DbSet<TipoPrecio> TipoPrecios { get; set; }
        public DbSet<TiqueteDescuento> TiqueteDescuentos { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Bitacora> Bitacoras { get; set; }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }

    }
}
