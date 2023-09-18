using System.Formats.Cbor;
using Telegram.Bot;
using Websocket.Client;

namespace CatMessenger.Telegram.Connector.Payloads;

public class ServerLifecyclePayload : MessengerPayloadBase
{
    private bool Started { get; }
    
    public ServerLifecyclePayload(bool isStarted)
    {
        Started = isStarted;
    }

    public ServerLifecyclePayload(CborReader reader) : base(reader)
    {
        Started = reader.ReadBoolean();
    }
    
    public override MessengerPayloadType GetPayloadType()
    {
        return MessengerPayloadType.ServerLifecycle;
    }

    protected override void Write(CborWriter writer)
    {
        writer.WriteBoolean(Started);
    }

    public override string AsString()
    {
        var text = Started ? "启动" : "关闭";
        return $"服务器{text}了";
    }

    public override async Task Handle(string publisher, WebsocketClient client, ServerPacketHandler packetHandler)
    {
        var config = packetHandler.Config;
        var text = Started ? "启动" : "关闭";
        await packetHandler.Bot.SendTextMessageAsync(config.GetTelegramChatId(), $"服务器 {publisher} {text}了");
    }
}