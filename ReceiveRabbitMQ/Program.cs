using DotNetEnv;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ReceiveRabbitMQ;
using ReceiveRabbitMQ.Senders;

// Для локальной работы вернуть
//Env.Load();

IConfigurationRoot configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .AddEnvironmentVariables()
    .Build();

var serviceProvider = new ServiceCollection()
            .AddSingleton<IConfiguration>(configuration)
            .AddSingleton<IEmailSender, EmailSender>()
            .AddSingleton<Receive>()
            .BuildServiceProvider();

var receive = serviceProvider.GetRequiredService<Receive>();

CancellationTokenSource cts = new CancellationTokenSource();

Console.CancelKeyPress += (s, e) =>
{
    Console.WriteLine("Press [enter] to exit.");

    cts.Cancel();

    e.Cancel = true;
};

await receive.StartConsumeAsync(cts.Token);
