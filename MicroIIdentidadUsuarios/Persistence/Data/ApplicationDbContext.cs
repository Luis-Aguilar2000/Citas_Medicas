using Domain.Models;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Persistence.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(
            DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Rol> Roles { get; set; }
        public DbSet<Permisos> Permisos { get; set; }
        public DbSet<RolPermiso> RolPermisos { get; set; }

        public DbSet<Medico> Medicos { get; set; }
        public DbSet<Especialidad> Especialidades { get; set; }
        public DbSet<MedicoEspecialidad> MedicoEspecialidades { get; set; }

        public DbSet<SesionUsuario> SesionesUsuario { get; set; }

        protected override void OnModelCreating(
            ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(
                Assembly.GetExecutingAssembly()
            );
        }
    }
}