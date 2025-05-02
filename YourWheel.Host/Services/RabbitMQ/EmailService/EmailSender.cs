using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMqConfiguration;
using System.Text;
using YourWheel.Host.Logging;

namespace YourWheel.Host.Services.RabbitMQ.EmailService
{
    public class EmailSender : IEmailSender
    {
        private readonly string _hostName;

        private readonly string _queueName;

        private IConnection _connection;

        public EmailSender()
        {
            this._hostName = ConfigurationHelper.Configuration.GetValue<string>("RabbitMQ:HostName");

            this._queueName = ConfigurationHelper.Configuration.GetValue<string>("RabbitMQ:QueueName");
        }

        public async Task SendEmailAsync(string toEmail, string subject, string body)
        {
            if (this._connection == null || !this._connection.IsOpen) 
                await this.CreateConnection();

            // Каждая операция протокола, выполняемая клиентом, происходит в отдельном канале.
            using (var channel = await this._connection.CreateChannelAsync())
            {
                await channel.QueueDeclareAsync(queue: _queueName, durable: true, exclusive: false, autoDelete: false, arguments: null);

                RabbitMQMessage mQMessage = new RabbitMQMessage()
                {
                    Body = body,
                    Subject = subject,
                    ToEmail = toEmail
                };

                var json = JsonConvert.SerializeObject(mQMessage);

                var bodyBytes = Encoding.UTF8.GetBytes(json);

                await channel.BasicPublishAsync(string.Empty, this._queueName, false, bodyBytes);

                Log.Info($" [x] Sent email message To: {toEmail}");
            }
        }

        private async Task CreateConnection()
        {
            try
            {
                var factory = new ConnectionFactory
                {
                    HostName = this._hostName
                };

                this._connection = await factory.CreateConnectionAsync();

                Log.Info("RabbitMQ connection created");
            }
            catch (Exception ex)
            {
                Log.Error("Could not create RabbitMQ connection");

                throw;
            }
        }
    }
}
