using Core.Facturacion.Services;
using Infraestructura.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NugetPackage_Rest.Builders;
using NugetPackage_Rest.Extensions;
using NugetPackage_Rest.Interfaces.IServices;
using System;

namespace Infraestructura
{
    public static class Extension
    {
        public static IServiceCollection AddExternals(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddRequestLogging(configuration);

            services.AddHttpClient<IRest, RestBuilder>(client =>
            {
                client.Timeout = TimeSpan.FromSeconds(30);
            });

            services.AddScoped<IPacienteService, PacienteService>();
            services.AddScoped<ITratamientoLabService, TratamientoLabService>();

            return services;
        }
    }
}