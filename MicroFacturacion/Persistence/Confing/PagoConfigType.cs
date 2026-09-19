using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Facturacion.Config
{
    public class PagoConfigType
        : IEntityTypeConfiguration<Pago>
    {
        public void Configure(
            EntityTypeBuilder<Pago> builder)
        {
            builder.HasKey(x => x.IdPago);

            builder.Property(x => x.Monto)
                .HasPrecision(18, 2);

            builder.HasOne<Factura>()
                .WithMany()
                .HasForeignKey(x => x.IdFactura)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne<FormaPago>()
                .WithMany()
                .HasForeignKey(x => x.IdFormaPago)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}