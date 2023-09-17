using CatMessenger.Telegram.Bot.Bases;
using Microsoft.Extensions.Logging;
using Telegram.Bot;

namespace CatMessenger.Telegram.Bot;

public class ReceiverService : ReceiverServiceBase<UpdateHandler>
{
    public ReceiverService(ITelegramBotClient bot, UpdateHandler updateHandler, ILogger<ReceiverService> logger) 
        : base(bot, updateHandler, logger)
    {
    }
}