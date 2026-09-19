using Domain.Facturacion.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Facturacion.Config
{
    public class ServicioConfigType
        : IEntityTypeConfiguration<Servicio>
    {
        public void Configure(
            EntityTypeBuilder<Servicio> builder)
        {
            builder.HasKey(x => x.IdServicio);
        }
    }
}