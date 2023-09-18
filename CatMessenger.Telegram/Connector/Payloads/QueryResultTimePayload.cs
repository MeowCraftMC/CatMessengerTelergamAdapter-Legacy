using System.Formats.Cbor;
using System.Text;
using CatMessenger.Telegram.Bot;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;
using Websocket.Client;

namespace CatMessenger.Telegram.Connector.Payloads;

public class QueryResultTimePayload : MessengerPayloadBase
{
    private string ServerName { get; }
    private string WorldName { get; }
    private int DayTime { get; }
    private int GameTime { get; }
    private int Day { get; }
    
    public QueryResultTimePayload(string serverName, string worldName, int dayTime, int gameTime, int day)
    {
        ServerName = serverName;
        WorldName = worldName;
        DayTime = dayTime;
        GameTime = gameTime;
        Day = day;
    }

    public QueryResultTimePayload(CborReader reader)
    {
        ServerName = reader.ReadTextString();
        WorldName = reader.ReadTextString();
        DayTime = reader.ReadInt32();
        GameTime = reader.ReadInt32();
        Day = reader.ReadInt32();
    } 
    
    public override MessengerPayloadType GetPayloadType()
    {
        return MessengerPayloadType.QueryResultTime;
    }

    protected override void Write(CborWriter writer)
    {
        writer.WriteTextString(ServerName);
        writer.WriteTextString(WorldName);
        writer.WriteInt32(DayTime);
        writer.WriteInt32(GameTime);
        writer.WriteInt32(Day);
    }

    public override string AsString()
    {
        return $"Time: {DayTime}/{GameTime}/{Day}({ServerName}/{WorldName})";
    }

    public override async Task Handle(string publisher, WebsocketClient client, ServerPacketHandler packetHandler)
    {
        if (publisher != ServerName)
        {
            return;
        }

        var request = packetHandler.Bot.QueryTimeRequest(ServerName, WorldName);
        if (request == null)
        {
            return;
        }

        var type = request.Value.Type;
        var reply = request.Value.ReplyTo;
        
        var config = packetHandler.Config;

        var result = string.Empty;
        if (type == TelegramBotClientExtension.TimeQueryType.DayTime)
        {
            result = DayTime switch
            {
                >= 0 and < 12000 => $"【{publisher}】白天 ({DayTime})",
                >= 12000 and < 13800 => $"【{publisher}】日落 ({DayTime})",
                >= 13800 and < 18000 => $"【{publisher}】夜晚 ({DayTime})",
                >= 18000 and < 24000 => $"【{publisher}】黎明 ({DayTime})",
                _ => $"【{publisher}】时间：{DayTime}"
            };
        }
        else if (type == TelegramBotClientExtension.TimeQueryType.GameTime)
        {
            result = $"【{publisher}】此世界已经运行了 {GameTime} 刻。";
        }
        else if (type == TelegramBotClientExtension.TimeQueryType.Day)
        {
            result = $"【{publisher}】此世界已经度过了 {Day} 天";
        }
        
        await packetHandler.Bot.SendTextMessageAsync(config.GetTelegramChatId(), 
            result, parseMode: ParseMode.Html, replyToMessageId: reply);
    }
}