using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Config
{
    public class RolPermisoConfig
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
        }
    }
}