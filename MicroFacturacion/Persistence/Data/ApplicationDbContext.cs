using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Facturacion.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(
            DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Servicio> Servicios { get; set; }

        public DbSet<Factura> Facturas { get; set; }

        public DbSet<FacturaDetalle> FacturaDetalles { get; set; }

        public DbSet<Pago> Pagos { get; set; }

        public DbSet<FormaPago> FormasPago { get; set; }

        protected override void OnModelCreating(
            ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // =========================
            // SERVICIO
            // =========================

            modelBuilder.Entity<Servicio>()
                .HasKey(x => x.IdServicio);

            modelBuilder.Entity<Servicio>()
                .Property(x => x.Precio)
                .HasPrecision(18, 2);


            // =========================
            // FACTURA
            // =========================

            modelBuilder.Entity<Factura>()
                .HasKey(x => x.IdFactura);


            // =========================
            // FACTURA DETALLE
            // =========================

            modelBuilder.Entity<FacturaDetalle>()
                .HasKey(x => x.IdDetalle);

            modelBuilder.Entity<FacturaDetalle>()
                .Property(x => x.PrecioUnitario)
                .HasPrecision(18, 2);

            modelBuilder.Entity<FacturaDetalle>()
                .HasOne<Factura>()
                .WithMany()
                .HasForeignKey(x => x.IdFactura)
                .OnDelete(DeleteBehavior.Restrict);


            // =========================
            // FORMA DE PAGO
            // =========================

            modelBuilder.Entity<FormaPago>()
                .HasKey(x => x.IdFormaPago);


            // =========================
            // PAGO
            // =========================

            modelBuilder.Entity<Pago>()
                .HasKey(x => x.IdPago);

            modelBuilder.Entity<Pago>()
                .Property(x => x.Monto)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Pago>()
                .HasOne<Factura>()
                .WithMany()
                .HasForeignKey(x => x.IdFactura)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Pago>()
                .HasOne<FormaPago>()
                .WithMany()
                .HasForeignKey(x => x.IdFormaPago)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}