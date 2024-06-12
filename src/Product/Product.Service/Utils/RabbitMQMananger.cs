using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Product.Domain.Interfaces.Utils;
using Product.Domain.Settings;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Product.Service.Utils
{
    public class RabbitMQManager : IRabbitMQManager
    {
        public IConnectionFactory ConnectionFactory { get; set; }
        public RabbitMQSettings RabbitSettings { get; set; }

        public RabbitMQManager(IConfiguration config)
        {
            var rabbitSection = config.GetSection(nameof(RabbitMQSettings));
            RabbitSettings = rabbitSection.Get<RabbitMQSettings>();
            ConnectionFactory = CreateConnectionFactory();
        }

        public void BasicPublish(ExecutionQueue queue, object message, IModel? channel, string rountingKey = "")
        {
            if (message is null || channel is null)
                return;

            var settings = channel.CreateBasicProperties();
            settings.ContentType = "application/json";
            channel.BasicPublish(exchange: GetQueueName(queue), rountingKey, settings, Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(message)));
        }

        public string GetQueueName(ExecutionQueue process)
        {
            switch (process)
            {
                case ExecutionQueue.Notification:
                    return RabbitSettings.NotificationQueue;
                default:
                    return string.Empty;
            }
        }

        public void CreateQueueExchange(IModel channel, string queue, string rountingKey)
        {
            channel.ExchangeDeclare(queue, ExchangeType.Topic, true, false, null);
            channel.QueueDeclare(queue, true, false, false, null);
            channel.QueueBind(queue, queue, rountingKey, null);
        }

        public IConnectionFactory CreateConnectionFactory() => new ConnectionFactory
        {
            HostName = RabbitSettings.HostName,
            UserName = RabbitSettings.UserName,
            Password = RabbitSettings.Password,
            VirtualHost = RabbitSettings.VirtualHost
        };
    }
}
