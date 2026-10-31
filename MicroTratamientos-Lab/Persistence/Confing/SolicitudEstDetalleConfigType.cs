using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Confing
{
    public class SolicitudEstDetalleConfigType
        : IEntityTypeConfiguration<SolicitudEstudioDetalle>
    {
        public void Configure(EntityTypeBuilder<SolicitudEstudioDetalle> builder)
        {
            builder.HasKey(x => x.IdSolicitudEstudioDetalle);

            builder.HasOne(x => x.SolicitudEstudio)
                .WithMany(x => x.Detalles)
                .HasForeignKey(x => x.IdSolicitudEstudio)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.CatalogoEstudio)
                .WithMany(x => x.SolicitudEstudioDetalles)
                .HasForeignKey(x => x.IdCatalogoEstudio)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}