using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Facturacion.Config
{
    public class FacturaConfigType
        : IEntityTypeConfiguration<Factura>
    {
        public void Configure(
            EntityTypeBuilder<Factura> builder)
        {
            builder.HasKey(x => x.IdFactura);
        }
    }
}