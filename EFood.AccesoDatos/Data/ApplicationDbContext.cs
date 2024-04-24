using EFood.Modelos;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace EFood.AccesoDatos.Data
{
    public class ApplicationDbContext : IdentityDbContext<Usuario>
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

    }
}
