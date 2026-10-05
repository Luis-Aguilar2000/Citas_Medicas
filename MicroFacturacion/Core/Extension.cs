using Core.Facturacion.Configs;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Core.Facturacion
{
    public static class Extension
    {
        public static IServiceCollection AddCore(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddMediatR(cfg =>
                cfg.RegisterServicesFromAssembly(
                    Assembly.GetExecutingAssembly()));

            services.Configure<DownstreamOptions>(
                configuration.GetSection("Downstream"));

            return services;
        }
    }
}