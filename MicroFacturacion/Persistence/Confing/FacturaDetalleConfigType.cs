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
        }
    }
}