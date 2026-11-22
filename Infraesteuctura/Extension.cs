using Core.Configs;
using Core.Services;
using Infraestructura.Services;
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
            services.AddRequestLogging(configuration);

            services.Configure<DownstreamOptions>(
                configuration.GetSection("Downstream"));

            services.AddHttpClient<IRest, RestBuilder>(client =>
            {
                client.Timeout = TimeSpan.FromSeconds(30);
            });

            services.AddScoped<IIdentidadService, IdentidadService>();

            return services;
        }
    }
}