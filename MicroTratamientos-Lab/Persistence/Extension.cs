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
<<<<<<< Updated upstream
            this IServiceCollection services,
            IConfiguration configuration)
=======
            this IServiceCollection services)
>>>>>>> Stashed changes
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    configuration["sql:cx"]
                )
            );

            services.AddScoped<DbContext>(provider =>
                provider.GetRequiredService<ApplicationDbContext>()
            );

            services.AddScoped(
                typeof(IGenericRepository<>),
                typeof(GenericRepository<>)
            );

            return services;
        }

    }
<<<<<<< Updated upstream
}
=======
}

>>>>>>> Stashed changes
