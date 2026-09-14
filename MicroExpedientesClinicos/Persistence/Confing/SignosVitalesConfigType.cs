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
    public class SignosVitalesConfigType : IEntityTypeConfiguration<SignosVitales>
    {
        public void Configure(EntityTypeBuilder<SignosVitales> builder)
        {
            throw new NotImplementedException();
        }
    }
}
