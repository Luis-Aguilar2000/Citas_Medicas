using Domain.Facturacion.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Facturacion.Config
{
    public class FormaPagoConfigType
        : IEntityTypeConfiguration<FormaPago>
    {
        public void Configure(
            EntityTypeBuilder<FormaPago> builder)
        {
            builder.HasKey(x => x.IdFormaPago);
        }
    }
}