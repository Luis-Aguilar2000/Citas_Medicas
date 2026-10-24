using Core.Features.Auth.Interfaces;
using Infraestructura.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Infraestructura
{
    public static class Extension
    {
        public static IServiceCollection AddInfraestructura(
            this IServiceCollection services)
        {
            services.AddScoped<IJwtService, JwtService>();

            return services;
        }
    }
}