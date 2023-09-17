using System.Net;
using CatMessenger.Telegram;
using CatMessenger.Telegram.Bot;
using CatMessenger.Telegram.Bot.Bases;
using CatMessenger.Telegram.Connector;
using CatMessenger.Telegram.Connector.Packet.S2C;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;
using Telegram.Bot;

var builder = Host.CreateApplicationBuilder(args);

builder.Configuration.AddJsonFile("config.json", optional: true, reloadOnChange: true)
    .AddJsonFile($"config.{ConfigAccessor.GetDevEnvironmentVariable()}.json", optional: true, reloadOnChange: true)
    .AddCommandLine(args)
    .AddEnvironmentVariables();

builder.Logging.ClearProviders()
    .SetMinimumLevel(LogLevel.Trace)
    .AddNLog();

var config = new ConfigAccessor(builder.Configuration);
builder.Services.AddSingleton(config);

builder.Services.AddHttpClient("TelegramBotClient")
    .ConfigurePrimaryHttpMessageHandler(() =>
    {
        var httpClientHandler = new HttpClientHandler();

        if (config.IsTelegramProxyEnabled())
        {
            var url = config.GetTelegramProxyUrl();
            if (string.IsNullOrWhiteSpace(url))
            {
                throw new ArgumentException("Config section Telegram:Proxy:Url should not be null.");
            }
            
            httpClientHandler.Proxy = new WebProxy
            {
                Address = new Uri(url)
            };
        }

        return httpClientHandler;
    })
    .RemoveAllLoggers()
    .AddTypedClient<ITelegramBotClient>((client, _) =>
    {
        var options = new TelegramBotClientOptions(token: config.GetTelegramToken());
        return new TelegramBotClient(options, client);
    });

builder.Services.AddSingleton<ServerPacketHandler>();
builder.Services.AddHostedService<ConnectorClientService>();

// Fixme: qyl27: Maybe we needn't multi bot in same host?
builder.Services.AddScoped<UpdateHandler>();
builder.Services.AddScoped<ReceiverService>();
// builder.Services.AddSingleton<UpdateHandler>();
// builder.Services.AddSingleton<ReceiverService>();
builder.Services.AddHostedService<PollingService>();

using var host = builder.Build();

await host.RunAsync();
