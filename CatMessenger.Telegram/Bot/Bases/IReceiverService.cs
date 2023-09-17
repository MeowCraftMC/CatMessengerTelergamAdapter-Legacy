namespace CatMessenger.Telegram.Bot.Bases;

public interface IReceiverService
{
    Task ReceiveAsync(CancellationToken stoppingToken);
}