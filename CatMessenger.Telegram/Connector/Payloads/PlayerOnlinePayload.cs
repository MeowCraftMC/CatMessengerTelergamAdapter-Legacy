using System.Formats.Cbor;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;
using Websocket.Client;

namespace CatMessenger.Telegram.Connector.Payloads;

public class PlayerOnlinePayload : MessengerPayloadBase
{
    private bool Online { get; }
    private string PlayerName { get; }
    
    public PlayerOnlinePayload(bool isOnline, string playerName)
    {
        Online = isOnline;
        PlayerName = playerName;
    }

    public PlayerOnlinePayload(CborReader reader) : base(reader)
    {
        Online = reader.ReadBoolean();
        PlayerName = reader.ReadTextString();
    }
    
    public override MessengerPayloadType GetPayloadType()
    {
        return MessengerPayloadType.PlayerOnline;
    }

    protected override void Write(CborWriter writer)
    {
        writer.WriteBoolean(Online);
        writer.WriteTextString(PlayerName);
    }

    public override string AsString()
    {
        var text = Online ? "进入" : "退出";
        return $"{PlayerName} {text}了服务器";
    }
    
    public override async Task Handle(string publisher, WebsocketClient client, ServerPacketHandler packetHandler)
    {
        var config = packetHandler.Config;
        var text = Online ? "进入" : "退出";
        await packetHandler.Bot.SendTextMessageAsync(config.GetTelegramChatId(), 
            $"<b>{PlayerName}</b> {text}了 {publisher} 服务器", parseMode: ParseMode.Html);
    }
}