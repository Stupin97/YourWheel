using System.Net.Mail;

namespace YourWheel.Host.Services.Senders
{
    public class EmailSender : IEmailSender
    {
        public async Task SendEmailAsync(string toEmail, string subject, string body)
        {
            return;
        }
    }
}