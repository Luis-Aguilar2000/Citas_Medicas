using Generics.Interfaces;
using Generics.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Persistence.Data;

namespace Persistence
{
    public static class Extension
    {
        public static IServiceCollection AddPersistence(
            this IServiceCollection services)
        {
            IConfiguration configuration;

            using (ServiceProvider provider = services.BuildServiceProvider())
            {
                configuration =
                    ServiceProviderServiceExtensions
                    .GetService<IConfiguration>(provider)!;
            }

            services.AddDbContext<ApplicationDbContext>(
                opt => opt.UseSqlServer(configuration["sql:cx"])
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
