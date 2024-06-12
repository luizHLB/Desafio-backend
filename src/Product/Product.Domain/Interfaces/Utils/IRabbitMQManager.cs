using Product.Domain.Settings;
using RabbitMQ.Client;

namespace Product.Domain.Interfaces.Utils
{
    public interface IRabbitMQManager
    {
        public IConnectionFactory ConnectionFactory { get; set; }
        public RabbitMQSettings RabbitSettings { get; set; }

        void BasicPublish(ExecutionQueue queue, object message, IModel? channel, string rountingKey = "");
        string GetQueueName(ExecutionQueue process);
        void CreateQueueExchange(IModel channel, string queue, string rountingKey);
    }
}
