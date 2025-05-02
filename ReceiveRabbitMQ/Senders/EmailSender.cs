namespace ReceiveRabbitMQ.Senders
{
    using MailKit.Net.Smtp;
    using Microsoft.Extensions.Configuration;
    using MimeKit;
    using RabbitMqConfiguration;

    public class EmailSender : IEmailSender
    {
        private readonly string _host;

        private readonly int _port;

        private readonly string _username;

        private readonly string _appPassword;

        public EmailSender(IConfiguration configuration)
        {
            this._host = configuration.GetValue<string>("EmailSettings:Host");

            this._port = Convert.ToInt32(configuration.GetValue<string>("EmailSettings:Port"));

            this._username = configuration.GetValue<string>("EmailSettings:Username");

            this._appPassword = configuration.GetValue<string>("EmailSettings:AppPassword");
        }

        public async Task SendEmailAsync(RabbitMQMessage rabbitMQMessage)
        {
            var message = new MimeMessage();

            message.From.Add(new MailboxAddress("Your Wheel", this._username));

            message.To.Add(new MailboxAddress("", rabbitMQMessage.ToEmail));

            message.Subject = rabbitMQMessage.Subject;

            var builder = new BodyBuilder
            {
                HtmlBody = rabbitMQMessage.Body
            };

            message.Body = builder.ToMessageBody();

            message.Body.Prepare(MimeKit.EncodingConstraint.EightBit);

            // Добавить обработку исключений!

            using (var client = new SmtpClient())
            {
                await client.ConnectAsync(this._host,
                    this._port,
                    MailKit.Security.SecureSocketOptions.StartTls);

                await client.AuthenticateAsync(
                    this._username,
                    this._appPassword
                    );

                await client.SendAsync(message);

                await client.DisconnectAsync(true);
            }
        }
    }
}
