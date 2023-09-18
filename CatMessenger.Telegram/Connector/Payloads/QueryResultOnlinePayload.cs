using System.Formats.Cbor;
using System.Text;
using CatMessenger.Telegram.Bot;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;
using Websocket.Client;

namespace CatMessenger.Telegram.Connector.Payloads;

public class QueryResultOnlinePayload : MessengerPayloadBase
{
    private string ServerName { get; }
    private int PlayerCount { get; }
    private List<string> PlayerNames { get; }
    
    public QueryResultOnlinePayload(string serverName, List<string> playerNames)
    {
        ServerName = serverName;
        PlayerCount = playerNames.Count;
        PlayerNames = playerNames;
    }

    public QueryResultOnlinePayload(CborReader reader)
    {
        ServerName = reader.ReadTextString();
        PlayerCount = reader.ReadInt32();
        
        PlayerNames = new List<string>();
        for (var i = 0; i < PlayerCount; i++)
        {
            PlayerNames.Add(reader.ReadTextString());
        }
    } 
    
    public override MessengerPayloadType GetPayloadType()
    {
        return MessengerPayloadType.QueryResultOnline;
    }

    protected override void Write(CborWriter writer)
    {
        writer.WriteTextString(ServerName);
        writer.WriteInt32(PlayerCount);
        foreach (var name in PlayerNames)
        {
            writer.WriteTextString(name);
        }
    }

    public override string AsString()
    {
        return $"PlayersOnline: {PlayerCount}({ServerName})";
    }

    public override async Task Handle(string publisher, WebsocketClient client, ServerPacketHandler packetHandler)
    {
        if (publisher != ServerName)
        {
            return;
        }

        var reply = packetHandler.Bot.QueryOnlineRequest(publisher);
        if (reply == null)
        {
            return;
        }
        
        var config = packetHandler.Config;
        if (PlayerCount > 0)
        {
            var builder = new StringBuilder($"【{publisher}】目前 <b>{PlayerCount}</b> 位玩家在线：\n");
            for (var i = 1; i <= PlayerNames.Count; i++)
            {
                var name = PlayerNames[i];
                builder.Append($"{i}. {name}\n");
            }

            await packetHandler.Bot.SendTextMessageAsync(config.GetTelegramChatId(), 
                builder.ToString(), parseMode: ParseMode.Html, replyToMessageId: reply);
        }
        else
        {
            await packetHandler.Bot.SendTextMessageAsync(config.GetTelegramChatId(), 
                $"【{publisher}】<b>没人在线</b>", parseMode: ParseMode.Html, replyToMessageId: reply);
        }
    }
}