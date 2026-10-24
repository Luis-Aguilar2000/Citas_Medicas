using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Config
{
    public class SesionUsuarioConfigType
        : IEntityTypeConfiguration<SesionUsuario>
    {
        public void Configure(
            EntityTypeBuilder<SesionUsuario> builder)
        {
            builder.HasKey(x => x.IdSesion);

            builder.HasOne<Usuario>()
                .WithMany()
                .HasForeignKey(x => x.IdUsuario)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}