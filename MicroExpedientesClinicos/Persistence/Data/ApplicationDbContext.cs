using Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Data
{
    public class ApplicationDbContext: DbContext
    {

        public DbSet<Alergias> Alergias { get; set; }
        public DbSet<AntecedentesFamiliares> AntecedentesFamiliares { get; set; }
        public DbSet<AntecedentesMedicos> AntecedentesMedicos { get; set; }
        public DbSet<AtencionDiagnostico> AtencionDiagnosticos { get; set; }
        public DbSet<Atenciones> Atenciones { get; set; }
        public DbSet<AtencionSintoma> AtencionSintomas { get; set; }
        public DbSet<Diagnosticos> Diagnosticos { get; set; }
        public DbSet<ExpedientesClinicos> ExpedientesClinicos { get; set; }
        public DbSet<Habitos> habitos { get; set; }
        public DbSet<NotasMedicas> NotasMedicas { get; set; }
        public DbSet<SignosVitales> SignosVitales { get; set; }
        public DbSet<Sintomas> Sintomas { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}
