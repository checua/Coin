using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Coin.Models;

namespace Coin.Data
{
    public class CoinDbContext : IdentityDbContext<Usuario>
    {
        public CoinDbContext(DbContextOptions<CoinDbContext> options) : base(options) { }

        public DbSet<Usuario> COIN_Usuarios { get; set; }
        public DbSet<CuentaInterna> COIN_CuentasInternas { get; set; }
        public DbSet<Apuesta> COIN_Apuestas { get; set; }
        public DbSet<Transaccion> COIN_Transacciones { get; set; }
        public DbSet<Sistema> COIN_Sistema { get; set; } // Registrar el modelo

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Usuario>().ToTable("COIN_Usuarios").HasKey(u => u.Id);
            modelBuilder.Entity<CuentaInterna>().ToTable("COIN_CuentasInternas").HasKey(c => c.IdCuenta);
            modelBuilder.Entity<Apuesta>().ToTable("COIN_Apuestas").HasKey(a => a.IdApuesta);
            modelBuilder.Entity<Transaccion>().ToTable("COIN_Transacciones").HasKey(t => t.IdTransaccion);
            modelBuilder.Entity<Sistema>().ToTable("COIN_Sistema").HasKey(s => s.IdTransaccion); // Relacionar modelo con tabla
        }
    }
}
