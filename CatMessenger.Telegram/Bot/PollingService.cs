using CatMessenger.Telegram.Bot.Bases;
using Microsoft.Extensions.Logging;
using Telegram.Bot;

namespace CatMessenger.Telegram.Bot;

public class PollingService : PollingServiceBase<ReceiverService>
{
    private ITelegramBotClient Bot { get; }
    private ConfigAccessor Config { get; }
    
    public PollingService(IServiceProvider serviceProvider, ILogger<PollingService> logger,
        ITelegramBotClient bot,
        ConfigAccessor config) 
        : base(serviceProvider, logger)
    {
        Bot = bot;
        Config = config;
    }
    
    public override async Task StartAsync(CancellationToken cancellationToken)
    {
        await Bot.SendTextMessageAsync(Config.GetTelegramChatId(), "【系统】CatMessenger Telegram 适配器启动了！", cancellationToken: cancellationToken);
        await base.StartAsync(cancellationToken);
    }

    public override async Task StopAsync(CancellationToken cancellationToken)
    { 
        await Bot.SendTextMessageAsync(Config.GetTelegramChatId(), "【系统】CatMessenger Telegram 适配器关闭了！", cancellationToken: cancellationToken);
        await base.StopAsync(cancellationToken);
    }
}