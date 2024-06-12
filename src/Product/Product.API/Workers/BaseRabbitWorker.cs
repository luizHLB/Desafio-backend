using Product.Domain.Interfaces.Utils;
using Product.Domain.Settings;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace Product.API.Workers
{
    public abstract class BaseRabbitWorker : BackgroundService
    {
        protected readonly ILogger<BaseRabbitWorker> _logger;
        private readonly IRabbitMQManager _rabbitMQManager;
        private readonly string _routingKey;
        private readonly string _queue;

        public BaseRabbitWorker(ILogger<BaseRabbitWorker> logger, IRabbitMQManager rabbitMQManager, ExecutionQueue queue, string routingKey = null)
        {
            _logger = logger;
            _rabbitMQManager = rabbitMQManager;
            _routingKey = routingKey;
            _queue = _rabbitMQManager.GetQueueName(queue);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            using (var connection = _rabbitMQManager.ConnectionFactory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                _rabbitMQManager.CreateQueueExchange(channel, _queue, _routingKey);
                var consumer = new EventingBasicConsumer(channel);
                consumer.Received += async (sender, eventArgs) =>
                {
                    var message = Encoding.UTF8.GetString(eventArgs.Body.ToArray());
                    try
                    {
                        _logger.LogInformation($"Processing : {message}");
                        await Execute(message);
                        channel.BasicAck(eventArgs.DeliveryTag, false);
                    }
                    catch (Exception e)
                    {
                        _logger.LogError($"Process: {message} - Error: {e.Message}");
                        channel.BasicNack(eventArgs.DeliveryTag, false, true);
                    }
                };

                channel.BasicQos(0, 1, false);
                channel.BasicConsume(_queue, false, "Product.API", consumer);
                while (!stoppingToken.IsCancellationRequested)
                    await Task.Delay(1000, stoppingToken);
            }
        }

        protected abstract Task Execute(string message);
    }
}
