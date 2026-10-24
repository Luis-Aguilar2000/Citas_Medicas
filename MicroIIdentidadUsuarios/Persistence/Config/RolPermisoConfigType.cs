using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Config
{
    public class RolPermisoConfigType
        : IEntityTypeConfiguration<RolPermiso>
    {
        public void Configure(
            EntityTypeBuilder<RolPermiso> builder)
        {
            builder.HasKey(x => new
            {
                x.IdRol,
                x.IdPermiso
            });

            builder.HasOne<Rol>()
                .WithMany()
                .HasForeignKey(x => x.IdRol)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne<Permisos>()
                .WithMany()
                .HasForeignKey(x => x.IdPermiso)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}