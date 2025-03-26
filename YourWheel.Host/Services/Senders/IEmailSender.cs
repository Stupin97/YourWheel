namespace YourWheel.Host.Services.Senders
{
    public interface IEmailSender
    {
        /// <summary>
        /// Отправка письма
        /// </summary>
        /// <param name="toEmail">Получатель</param>
        /// <param name="subject">Тема</param>
        /// <param name="body">Тело письма</param>
        Task SendEmailAsync(string toEmail, string subject, string body);
    }
}
