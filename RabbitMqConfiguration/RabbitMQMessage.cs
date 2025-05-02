namespace RabbitMqConfiguration
{
    /// <summary>
    /// Принимаемое сообщение брокером для очереди sendAuth
    /// </summary>
    public class RabbitMQMessage
    {
        public string ToEmail { get; set; }

        public string Subject { get; set; }

        public string Body { get; set; }
    }
}
