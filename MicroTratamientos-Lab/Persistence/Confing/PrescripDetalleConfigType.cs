using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Confing
{
    public class PrescripDetalleConfigType
        : IEntityTypeConfiguration<PrescripcionDetalle>
    {
        public void Configure(EntityTypeBuilder<PrescripcionDetalle> builder)
        {
            builder.HasKey(x => x.IdPrescripcionDetalle);

            builder.Property(x => x.Dosis)
                .HasColumnType("decimal(10,2)")
                .IsRequired();

            builder.HasOne(x => x.Prescripcion)
                .WithMany(x => x.Detalles)
                .HasForeignKey(x => x.IdPrescripcion)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Medicamento)
                .WithMany(x => x.PrescripcionDetalles)
                .HasForeignKey(x => x.IdMedicamento)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}