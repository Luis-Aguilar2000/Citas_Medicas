using Microsoft.Extensions.DependencyInjection;

namespace Core
{
    public static class Extencion
    {
        public static IServiceCollection AddCore(this IServiceCollection services)
        {

            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
            return services;

        }

    }
}
