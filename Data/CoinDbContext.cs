using Microsoft.EntityFrameworkCore;
using Coin.Models;

namespace Coin.Data
{
    public class CoinDbContext : DbContext
    {
        public CoinDbContext(DbContextOptions<CoinDbContext> options) : base(options) { }

        public DbSet<Usuario> COIN_Usuarios { get; set; }
        public DbSet<CuentaInterna> COIN_CuentasInternas { get; set; }
        public DbSet<Apuesta> COIN_Apuestas { get; set; }
        public DbSet<Transaccion> COIN_Transacciones { get; set; }
        public DbSet<Sistema> COIN_Sistema { get; set; } // Registrar el modelo

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Usuario>().ToTable("COIN_Usuarios").HasKey(u => u.IdUsuario);
            modelBuilder.Entity<CuentaInterna>().ToTable("COIN_CuentasInternas").HasKey(c => c.IdCuenta);
            modelBuilder.Entity<Apuesta>().ToTable("COIN_Apuestas").HasKey(a => a.IdApuesta);
            modelBuilder.Entity<Transaccion>().ToTable("COIN_Transacciones").HasKey(t => t.IdTransaccion);
            modelBuilder.Entity<Sistema>().ToTable("COIN_Sistema").HasKey(s => s.IdTransaccion); // Relacionar modelo con tabla
        }
    }
}
