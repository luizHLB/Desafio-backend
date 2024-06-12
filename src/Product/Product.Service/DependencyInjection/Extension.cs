using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Product.Domain.Interfaces.Services;

namespace Product.Service.DependencyInjection
{
    public static class Extension
    {
        public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration config)
        { 
            services.AddScoped<IVehicleService, VehicleService>();

            return services;
        }
    }
}
