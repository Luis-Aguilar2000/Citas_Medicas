using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Config
{
    public class PermisosConfigType : IEntityTypeConfiguration<Permisos>
    {
        public void Configure(EntityTypeBuilder<Permisos> builder)
        {
            builder.HasKey(x => x.IdPermiso);
        }
    }
}