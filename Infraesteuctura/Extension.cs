using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NugetPackage_Rest.Builders;
using NugetPackage_Rest.Extensions;
using NugetPackage_Rest.Interfaces.IServices;

namespace Infraestructura
{
    public static class Extension
    {
        public static IServiceCollection AddExternals(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            // Lee la configuración del registro de peticiones.
            services.AddRequestLogging(configuration);

            // Registra la librería para enviar peticiones HTTP.
            services.AddHttpClient<IRest, RestBuilder>(client =>
            {
                client.Timeout = System.TimeSpan.FromSeconds(30);
            });

            return services;
        }
    }
}