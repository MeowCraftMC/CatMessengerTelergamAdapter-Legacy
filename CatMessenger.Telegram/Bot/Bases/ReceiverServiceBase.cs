using Microsoft.Extensions.Logging;
using Telegram.Bot;
using Telegram.Bot.Polling;

namespace CatMessenger.Telegram.Bot.Bases;

public abstract class ReceiverServiceBase<TUpdateHandler> : IReceiverService 
    where TUpdateHandler : IUpdateHandler
{
    private ITelegramBotClient Bot { get; }
    private TUpdateHandler UpdateHandler  { get; }
    private ILogger<ReceiverServiceBase<TUpdateHandler>> Logger  { get; }

    protected ReceiverServiceBase(
        ITelegramBotClient bot,
        TUpdateHandler updateHandler,
        ILogger<ReceiverServiceBase<TUpdateHandler>> logger)
    {
        Bot = bot;
        UpdateHandler = updateHandler;
        Logger = logger;
    }
    
    public async Task ReceiveAsync(CancellationToken stoppingToken)
    {
        var receiverOptions = new ReceiverOptions();

        var me = await Bot.GetMeAsync(stoppingToken);
        Logger.LogInformation("Start receiving updates for @{Name}", me.Username ?? "Telegram Bot");

        await Bot.ReceiveAsync(
            updateHandler: UpdateHandler,
            receiverOptions: receiverOptions,
            cancellationToken: stoppingToken);
    }
}