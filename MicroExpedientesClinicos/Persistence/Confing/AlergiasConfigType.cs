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
    public class AlergiasConfigType : IEntityTypeConfiguration<Alergias>
    {
        public void Configure(EntityTypeBuilder<Alergias> builder)
        {
            throw new NotImplementedException();
        }
    }
}
