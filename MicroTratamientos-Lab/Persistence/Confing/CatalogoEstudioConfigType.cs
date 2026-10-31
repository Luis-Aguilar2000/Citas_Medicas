using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Confing
{
    public class CatalogoEstudioConfigType : IEntityTypeConfiguration<CatalogoEstudio>
    {
        public void Configure(EntityTypeBuilder<CatalogoEstudio> builder)
        {
            builder.HasKey(x => x.IdCatalogoEstudio);

            builder.Property(x => x.Precio)
                .HasColumnType("decimal(10,2)")
                .IsRequired();
        }
    }
}