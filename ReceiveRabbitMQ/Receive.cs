using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMqConfiguration;
using ReceiveRabbitMQ.Senders;
using System.Text;
using System.Threading.Channels;

namespace ReceiveRabbitMQ
{
    public class Receive : IAsyncDisposable
    {
        private readonly string _hostName;

        private readonly string _queueName;

        private readonly IConfiguration _configuration;

        private readonly IEmailSender _sender;

        private IConnection _connection;

        public Receive(IConfiguration configuration, IEmailSender sender)
        {
            // Позже добавить логирование

            this._configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));

            this._sender = sender ?? throw new ArgumentNullException(nameof(sender));

            this._hostName = this._configuration.GetValue<string>("RabbitMQ:HostName") ?? throw new ArgumentNullException("RabbitMQ:HostName не задан");

            this._queueName = this._configuration.GetValue<string>("RabbitMQ:QueueName") ?? throw new ArgumentNullException("RabbitMQ:QueueName не задан");
        }

        public async Task StartConsumeAsync(CancellationToken cancellationToken)
        {
            if (this._connection == null || !this._connection.IsOpen)
                await this.CreateConnectionAsync(cancellationToken);

            // Каждая операция протокола, выполняемая клиентом, происходит в отдельном канале.
            using (var channel = await this._connection.CreateChannelAsync())
            {
                await channel.QueueDeclareAsync(queue: this._queueName, durable: true, exclusive: false, autoDelete: false, arguments: null);

                Console.WriteLine(" [*] Waiting for messages.");

                var consumer = new AsyncEventingBasicConsumer(channel);

                consumer.ReceivedAsync += async (model, ea) =>
                {
                    try
                    {
                        var body = ea.Body.ToArray();

                        var message = Encoding.UTF8.GetString(body);

                        RabbitMQMessage rabbitMQMessage = JsonConvert.DeserializeObject<RabbitMQMessage>(message);

                        await this._sender.SendEmailAsync(rabbitMQMessage);

                        channel.BasicAckAsync(deliveryTag: ea.DeliveryTag, multiple: false);

                        Console.WriteLine($" [x] Received {rabbitMQMessage?.ToEmail}");
                    }
                    catch (Exception exception)
                    {
                        Console.WriteLine($" [x] Error: {exception.Message}");

                        //Отправка сообщения в "dead-letter" очередь
                        channel.BasicNackAsync(deliveryTag: ea.DeliveryTag, multiple: false, requeue: false);

                        throw;
                    }
                };

                await channel.BasicConsumeAsync(this._queueName, autoAck: false, consumer: consumer);

                while (!cancellationToken.IsCancellationRequested)
                {
                    await Task.Delay(TimeSpan.FromSeconds(1), cancellationToken);
                }

                Console.WriteLine(" [*] Cancellation requested.");
            }
        }

        async ValueTask IAsyncDisposable.DisposeAsync()
        {
            try
            {
                if (this._connection != null && this._connection.IsOpen)
                {
                    await this._connection.CloseAsync();

                    this._connection?.Dispose();
                }
            }
            finally
            {
                this._connection = null;

                GC.SuppressFinalize(this);
            }
        }           

        private async Task CreateConnectionAsync(CancellationToken cancellationToken)
        {
            try
            {
                for (int indexTry = 0; indexTry < 3; indexTry++)
                {
                    try
                    {
                        if (cancellationToken.IsCancellationRequested) throw new OperationCanceledException("CancellationToken = true");

                        var factory = new ConnectionFactory
                        {
                            HostName = this._hostName
                        };

                        this._connection = await factory.CreateConnectionAsync(cancellationToken);

                        Console.WriteLine("RabbitMQ connection created");

                        break;
                    }
                    catch (OperationCanceledException ex)
                    {
                        throw;
                    }
                    catch (Exception exception) 
                    {
                        Console.WriteLine($"Could not create RabbitMQ Try: {indexTry} connection; {exception.Message}; HostName: {this._hostName}");

                        if (indexTry < 3)
                        {
                            Task.Delay(2000);

                            continue;
                        }

                        throw;
                    }
                }
            }
            catch (OperationCanceledException ex)
            {
                Console.WriteLine($"{ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Could not create RabbitMQ connection; {ex.Message}");
            }
        }
    }
}
