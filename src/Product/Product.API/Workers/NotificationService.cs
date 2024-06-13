using Newtonsoft.Json;
using Product.Domain.Entities;
using Product.Domain.Interfaces.Services;
using Product.Domain.Interfaces.Utils;
using Product.Domain.Settings;

namespace Product.API.Workers
{
    public class NotificationService : BaseRabbitWorker
    {
        private readonly IServiceProvider _serviceProvider;

        public NotificationService(ILogger<NotificationService> logger, IRabbitMQManager rabbitMQManager, IServiceProvider serviceProvider)
            : base(logger, rabbitMQManager, ExecutionQueue.Notification, "2024")
        {
            _serviceProvider = serviceProvider;
        }

        protected override async Task Execute(string message)
        {
            var dto = JsonConvert.DeserializeObject<Vehicle>(message);
            using(var scope = _serviceProvider.CreateScope())
            {
                var service = scope.ServiceProvider.GetService<INotificationService>();
                await service.Register(dto);
            }
        }
    }
}
