using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Confing
{
    public class MedicamentoConfigType : IEntityTypeConfiguration<Medicamento>
    {
        public void Configure(EntityTypeBuilder<Medicamento> builder)
        {
            builder.HasKey(x => x.IdMedicamento);

            builder.Property(x => x.Precio)
                .HasColumnType("decimal(10,2)")
                .IsRequired();
        }
    }
}