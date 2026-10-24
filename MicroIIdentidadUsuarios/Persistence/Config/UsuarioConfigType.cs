using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Config
{
    public class UsuarioConfigType
        : IEntityTypeConfiguration<Usuario>
    {
        public void Configure(
            EntityTypeBuilder<Usuario> builder)
        {
            builder.HasKey(x => x.IdUsuario);

            builder.HasOne<Rol>()
                .WithMany()
                .HasForeignKey(x => x.IdRol)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}