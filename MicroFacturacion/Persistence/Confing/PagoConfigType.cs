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
        }
    }
}