using System.Net.Mail;

namespace YourWheel.Host.Services.RabbitMQ.EmailService
{
    /// <summary>
    /// Сервис для отправки email через RabbitMQ
    /// </summary>
    public interface IEmailSender
    {
        /// <summary>
        /// Отправка сообщения через RabbitMQ
        /// </summary>
        /// <param name="toEmail"></param>
        /// <param name="subject"></param>
        /// <param name="body"></param>
        Task SendEmailAsync(string toEmail, string subject, string body);
    }
}
