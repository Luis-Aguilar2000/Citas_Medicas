using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Config
{
    public class MedicoEspecialidadConfigType
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

            builder.HasOne<Medico>()
                .WithMany()
                .HasForeignKey(x => x.IdMedico)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne<Especialidad>()
                .WithMany()
                .HasForeignKey(x => x.IdEspecialidad)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}