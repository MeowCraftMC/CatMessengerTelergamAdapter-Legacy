using CatMessenger.Telegram.Bot.Bases;
using CatMessenger.Telegram.Connector;
using Microsoft.Extensions.Logging;
using Telegram.Bot;

namespace CatMessenger.Telegram.Bot;

public class PollingService : PollingServiceBase<ReceiverService>
{
    private ITelegramBotClient Bot { get; }
    private ConfigAccessor Config { get; }
    private IConnectorClientService ConnectorClient { get; }
    
    public PollingService(IServiceProvider serviceProvider, ILogger<PollingService> logger,
        ITelegramBotClient bot,ConfigAccessor config, IConnectorClientService connectorClient) 
        : base(serviceProvider, logger)
    {
        Bot = bot;
        Config = config;
        ConnectorClient = connectorClient;
    }
    
    public override async Task StartAsync(CancellationToken cancellationToken)
    {
        ConnectorClient.Start();
        await Bot.SendTextMessageAsync(Config.GetTelegramChatId(), $"【系统】{Config.GetName()} 适配器启动了！", cancellationToken: cancellationToken);
        await base.StartAsync(cancellationToken);
    }

    public override async Task StopAsync(CancellationToken cancellationToken)
    {
        ConnectorClient.Stop();
        await Bot.SendTextMessageAsync(Config.GetTelegramChatId(), $"【系统】{Config.GetName()} 适配器关闭了！", cancellationToken: cancellationToken);
        await base.StopAsync(cancellationToken);
    }
}