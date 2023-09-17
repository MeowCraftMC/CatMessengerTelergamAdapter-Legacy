using System.Net;
using CatMessenger.Telegram.Connector;
using CatMessenger.Telegram.Utilities;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;

namespace CatMessenger.Telegram.Bot;

public class UpdateHandler : IUpdateHandler
{
    private ILogger<UpdateHandler> Logger { get; }
    private ConfigAccessor Config { get; }
    
    private ITelegramBotClient BotClient { get; }
    private IConnectorClientService ConnectorClient { get; }

    public UpdateHandler(ILogger<UpdateHandler> logger, ConfigAccessor config, ITelegramBotClient bot, 
        IConnectorClientService connectorClient)
    {
        Logger = logger;
        Config = config;
        
        BotClient = bot;
        ConnectorClient = connectorClient;
    }

    public Task HandleUpdateAsync(ITelegramBotClient _, Update update, CancellationToken cancellationToken)
    {
        ConnectorClient.SendChatMessage(MessageParser.FromUpdate(update).ToString());
        return Task.CompletedTask;
    }

    public Task HandlePollingErrorAsync(ITelegramBotClient _, Exception exception, CancellationToken cancellationToken)
    {
        Logger.LogWarning(exception, "Polling error!");
        return Task.CompletedTask;
    }
}