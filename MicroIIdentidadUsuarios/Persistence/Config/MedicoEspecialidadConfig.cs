using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Config
{
    public class MedicoEspecialidadConfig
        : IEntityTypeConfiguration<MedicoEspecialidad>
    {
        public void Configure(
            EntityTypeBuilder<MedicoEspecialidad> builder)
        {
            builder.HasKey(x => new
            {
                x.IdMedico,
                x.IdEspecialidad
            });
        }
    }
}