using CatMessenger.Telegram.Connector;
using CatMessenger.Telegram.Connector.Payloads;
using CatMessenger.Telegram.Utilities;
using Microsoft.Extensions.Logging;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

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

    public async Task HandleUpdateAsync(ITelegramBotClient _, Update update, CancellationToken cancellationToken)
    {
        if (update.Type == UpdateType.Message 
            && update.Message!.Type == MessageType.Text 
            && update.Message.Text!.StartsWith("/"))
        {
            var command = update.Message.Text.Substring(0, 1).Split(" ");
            await OnCommand(update.Message, command[0].Split('@')[0], command[1..]);
        }
        else
        {
            ConnectorClient.SendChatMessage(new ChatComponentPayload(MessageParser.FromUpdate(update).ToString()));
        }
    }

    public Task HandlePollingErrorAsync(ITelegramBotClient _, Exception exception, CancellationToken cancellationToken)
    {
        Logger.LogWarning(exception, "Polling error!");
        return Task.CompletedTask;
    }

    public async Task OnCommand(Message message, string command, params string[] args)
    {
        if (command == "online")
        {
            if (args.Length != 1)
            {
                await BotClient.SendTextMessageAsync(message.Chat.Id, "用法不正确。/online <服务器名>", 
                    replyToMessageId: message.MessageId);
                return;
            }
            
            BotClient.QueryRequestOnline(message.MessageId, args[0]);
            ConnectorClient.SendChatMessage(new QueryOnlinePayload(args[0]));   // Todo: qyl27: When server is not exists?
            return;
        }
        
        if (command == "time")
        {
            if (args.Length is > 3 or < 1)
            {
                await BotClient.SendTextMessageAsync(message.Chat.Id, "用法不正确！/time <服务器名> [世界名] [查询类型]", 
                    replyToMessageId: message.MessageId);
                return;
            }

            var world = args.Length >= 2 ? args[1] : "world";

            var typeStr = args.Length == 3 ? args[2] : "DayTime";
            var type = TelegramBotClientExtension.TimeQueryType.DayTime;
            if (typeStr == "GameTime")
            {
                type = TelegramBotClientExtension.TimeQueryType.GameTime;
            }
            else if (typeStr == "Day")
            {
                type = TelegramBotClientExtension.TimeQueryType.Day;
            }
            
            BotClient.QueryRequestTime(message.MessageId, args[0], world, type);
            ConnectorClient.SendChatMessage(new QueryTimePayload(args[0], world));   // Todo: qyl27: When server or world is not exists?
            return;
        }
    }
}