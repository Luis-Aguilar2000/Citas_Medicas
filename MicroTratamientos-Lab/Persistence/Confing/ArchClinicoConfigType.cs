using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Confing
{
    public class ArchClinicoConfigType : IEntityTypeConfiguration<ArchivoClinico>
    {
        public void Configure(EntityTypeBuilder<ArchivoClinico> builder)
        {
            builder.HasKey(x => x.IdArchivoClinico);

            builder.HasOne(x => x.ResultadoEstudio)
                .WithMany(x => x.ArchivosClinicos)
                .HasForeignKey(x => x.IdResultadoEstudio)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}