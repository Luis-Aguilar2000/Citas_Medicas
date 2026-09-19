using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Facturacion.Config
{
    public class FacturaDetalleConfigType
        : IEntityTypeConfiguration<FacturaDetalle>
    {
        public void Configure(
            EntityTypeBuilder<FacturaDetalle> builder)
        {
            builder.HasKey(x => x.IdDetalle);

            builder.Property(x => x.PrecioUnitario)
                .HasPrecision(18, 2);

            builder.HasOne<Factura>()
                .WithMany()
                .HasForeignKey(x => x.IdFactura)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}