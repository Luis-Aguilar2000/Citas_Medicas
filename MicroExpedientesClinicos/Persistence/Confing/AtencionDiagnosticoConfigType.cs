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
    public class AtencionDiagnosticoConfigType : IEntityTypeConfiguration<AtencionDiagnostico>
    {
        public void Configure(EntityTypeBuilder<AtencionDiagnostico> builder)
        {
            throw new NotImplementedException();
        }
    }
}
