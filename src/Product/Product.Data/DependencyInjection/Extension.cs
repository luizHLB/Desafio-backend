using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Product.Data.Contexts;
using Product.Data.Repositories;
using Product.Domain.Interfaces.Repositories;

namespace Product.Data.DependencyInjection
{
    public static class Extension
    {
        public static IServiceCollection AddRepositories(this IServiceCollection service, IConfiguration config)
        {
            service.AddScoped<IDriverRepository, DriverRepository>();
            service.AddScoped<IVehicleRepository, VehicleRepository>();

            service.AddDbContext<ProductContext>(options => options.UseNpgsql(config.GetConnectionString("ProductDB")));

            return service;
        }
    }
}
