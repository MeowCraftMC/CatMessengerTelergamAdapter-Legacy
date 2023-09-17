using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Telegram.Bot;

namespace CatMessenger.Telegram.Bot.Bases;

public abstract class PollingServiceBase<TReceiverService> : BackgroundService
    where TReceiverService : IReceiverService
{
    private IServiceProvider ServiceProvider { get; }
    private ILogger<PollingServiceBase<TReceiverService>> Logger { get; }
    
    protected PollingServiceBase(
        IServiceProvider serviceProvider,
        ILogger<PollingServiceBase<TReceiverService>> logger)
    {
        ServiceProvider = serviceProvider;
        Logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        Logger.LogInformation("Starting polling service");
        
        await DoReceive(stoppingToken);
    }

    private async Task DoReceive(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                // Fixme: qyl27: Maybe we needn't multi bot in same host?
                using var scope = ServiceProvider.CreateScope();
                var receiver = scope.ServiceProvider.GetRequiredService<TReceiverService>();
                // var receiver = ServiceProvider.GetService<TReceiverService>();
                
                await receiver!.ReceiveAsync(stoppingToken);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Polling failed!");
                await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken);
            }
        }
    }
}