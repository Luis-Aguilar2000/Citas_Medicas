using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Config
{
    public class MedicoConfigType
        : IEntityTypeConfiguration<Medico>
    {
        public void Configure(
            EntityTypeBuilder<Medico> builder)
        {
            builder.HasKey(x => x.IdMedico);

            builder.HasOne<Usuario>()
                .WithMany()
                .HasForeignKey(x => x.IdUsuario)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}