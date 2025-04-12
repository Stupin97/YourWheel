namespace YourWheel.Host.Services.Senders
{
    using MailKit.Net.Smtp;
    using MimeKit;

    public class EmailSender : IEmailSender
    {
        public async Task SendEmailAsync(string toEmail, string subject, string body)
        {
            var message = new MimeMessage();

            message.From.Add(new MailboxAddress("Your Wheel", ConfigurationHelper.Configuration.GetValue<string>("EmailSettings:Username")));

            message.To.Add(new MailboxAddress("", toEmail));

            message.Subject = subject;

            var builder = new BodyBuilder
            {
                HtmlBody = body
            };

            message.Body = builder.ToMessageBody();

            message.Body.Prepare(MimeKit.EncodingConstraint.EightBit);

            // Добавить обработку исключений!

            using (var client = new SmtpClient())
            {
                await client.ConnectAsync(ConfigurationHelper.Configuration.GetValue<string>("EmailSettings:Host"),
                    ConfigurationHelper.Configuration.GetValue<int>("EmailSettings:Port"),
                    MailKit.Security.SecureSocketOptions.StartTls);

                await client.AuthenticateAsync(
                    ConfigurationHelper.Configuration.GetValue<string>("EmailSettings:Username"),
                    ConfigurationHelper.Configuration.GetValue<string>("EmailSettings:AppPassword")
                    ); 

                await client.SendAsync(message);

                await client.DisconnectAsync(true);
            }
        }
    }
}