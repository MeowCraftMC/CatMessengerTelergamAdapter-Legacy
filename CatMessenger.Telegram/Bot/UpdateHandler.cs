using System.Net;
using CatMessenger.Telegram.Connector;
using CatMessenger.Telegram.Utilities;
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

    public UpdateHandler(ILogger<UpdateHandler> logger, ConfigAccessor config, ITelegramBotClient bot)
    {
        Logger = logger;
        Config = config;
        
        BotClient = bot;
    }

    public Task HandleUpdateAsync(ITelegramBotClient _, Update update, CancellationToken cancellationToken)
    {
        Logger.LogInformation(MessageParser.FromUpdate(update).ToString());
        return Task.CompletedTask;
    }

    public Task HandlePollingErrorAsync(ITelegramBotClient _, Exception exception, CancellationToken cancellationToken)
    {
        Logger.LogWarning(exception, "Polling error!");
        return Task.CompletedTask;
    }
}