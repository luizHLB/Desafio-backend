using Azure.Storage.Blobs;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Product.Domain.Interfaces.Services;
using Product.Domain.Interfaces.Utils;
using Product.Domain.Settings;
using Product.Service.Utils;

namespace Product.Service.DependencyInjection
{
    public static class Extension
    {
        public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration config)
        {
            var azureStorageSection = config.GetSection(nameof(AzureStorageSettings));
            var azureStorageSettings = azureStorageSection.Get<AzureStorageSettings>();
            services.AddScoped(serviceProvider =>
            {
                var response = new BlobContainerClient($"{azureStorageSettings.ConnectionString}{azureStorageSettings.BaseURI}", azureStorageSettings.ContainerName);
                response.CreateIfNotExistsAsync(Azure.Storage.Blobs.Models.PublicAccessType.BlobContainer);
                return response;
            });


            services.AddScoped<IVehicleService, VehicleService>();
            services.AddScoped<INotificationService, NotificationService>();
            services.AddScoped<IDriverService, DriverService>();
            services.AddScoped<IPlanService, PlanService>();
            services.AddScoped<IAzureStorage, AzureStorage>();

            services.AddSingleton<IRabbitMQManager, RabbitMQManager>();

            return services;
        }
    }
}
