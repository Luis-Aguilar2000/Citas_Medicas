using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Confing
{
    public class ResultadoEstudioConfigType
        : IEntityTypeConfiguration<ResultadoEstudio>
    {
        public void Configure(EntityTypeBuilder<ResultadoEstudio> builder)
        {
            builder.HasKey(x => x.IdResultadoEstudio);

            builder.HasOne(x => x.SolicitudEstudioDetalle)
                .WithOne(x => x.ResultadoEstudio)
                .HasForeignKey<ResultadoEstudio>(
                    x => x.IdSolicitudEstudioDetalle
                )
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}