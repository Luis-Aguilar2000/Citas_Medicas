using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Config
{
    public class SesionUsuarioConfig
        : IEntityTypeConfiguration<SesionUsuario>
    {
        public void Configure(
            EntityTypeBuilder<SesionUsuario> builder)
        {
            builder.HasKey(x => x.IdSesion);
        }
    }
}