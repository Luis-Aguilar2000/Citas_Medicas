using Domain.Facturacion.Models;
using Microsoft.EntityFrameworkCore;
using Persistence.Facturacion.Config;

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

            modelBuilder.ApplyConfiguration(
                new ServicioConfigType());

            modelBuilder.ApplyConfiguration(
                new FacturaConfigType());

            modelBuilder.ApplyConfiguration(
                new FacturaDetalleConfigType());

            modelBuilder.ApplyConfiguration(
                new PagoConfigType());

            modelBuilder.ApplyConfiguration(
                new FormaPagoConfigType());
        }
    }
}