using RabbitMqConfiguration;

namespace ReceiveRabbitMQ.Senders
{
    /// <summary>
    /// Сервис для отправки email через RabbitMQ
    /// </summary>
    public interface IEmailSender
    {
        /// <summary>
        /// Отправка письма
        /// </summary>
        /// <param name="rabbitMQMessage">message</param>
        Task SendEmailAsync(RabbitMQMessage rabbitMQMessage);
    }
}
