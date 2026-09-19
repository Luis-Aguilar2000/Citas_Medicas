using Generics.Interfaces;
using Generics.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Persistence.Facturacion.Data;

namespace Persistence.Facturacion
{
    public static class Extension
    {
        public static IServiceCollection AddPersistence(
            this IServiceCollection services)
        {
            IConfiguration configuration;

            using (ServiceProvider provider =
                services.BuildServiceProvider())
            {
                configuration =
                    ServiceProviderServiceExtensions
                    .GetService<IConfiguration>(provider)!;
            }

            services.AddDbContext<ApplicationDbContext>(
                opt => opt.UseSqlServer(
                    configuration["sql:cx"],
                    sqlOptions =>
                        sqlOptions.MigrationsHistoryTable(
                            "__EFMigrationsHistory_Facturacion")
                )
            );

            services.AddScoped<DbContext>(provider =>
                provider.GetRequiredService<ApplicationDbContext>());

            services.AddScoped(
                typeof(IGenericRepository<>),
                typeof(GenericRepository<>)
            );

            return services;
        }
    }
}