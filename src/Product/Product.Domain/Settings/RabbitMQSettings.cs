namespace Product.Domain.Settings
{
    public class RabbitMQSettings
    {
        public string HostName { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string VirtualHost { get; set; }

        public string NotificationQueue { get; set; }
    }

    public enum ExecutionQueue
    {
        Notification = 0
    }
}
