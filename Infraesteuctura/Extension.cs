using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infraestructura
{
    public static class Extension
    {
        public static IServiceCollection AddExternals(this IServiceCollection services)
        {

            IConfiguration configuration;
            using (ServiceProvider provider = services.BuildServiceProvider())
                configuration = ServiceProviderServiceExtensions.GetService<IConfiguration>(provider);

            return services;

        }
    }
}
