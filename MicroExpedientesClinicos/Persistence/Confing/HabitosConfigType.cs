using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Confing
{
    public class HabitosConfigType : IEntityTypeConfiguration<Habitos>
    {
        public void Configure(EntityTypeBuilder<Habitos> builder)
        {
            throw new NotImplementedException();
        }
    }
}
