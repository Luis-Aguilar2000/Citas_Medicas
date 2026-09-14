using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Confing
{
    public class ExpedienesClinicosConfigType : IEntityTypeConfiguration<ExpedientesClinicos>
    {
        public void Configure(EntityTypeBuilder<ExpedientesClinicos> builder)
        {
            throw new NotImplementedException();
        }
    }
}
