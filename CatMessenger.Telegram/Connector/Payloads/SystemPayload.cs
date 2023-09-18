using System.Formats.Cbor;
using Telegram.Bot;
using Websocket.Client;

namespace CatMessenger.Telegram.Connector.Payloads;

public class SystemPayload : MessengerPayloadBase
{
    private string Message { get; }
    
    public SystemPayload(string message)
    {
        Message = message;
    }

    public SystemPayload(CborReader reader) : base(reader)
    {
        Message = reader.ReadTextString();
    }
    
    public override MessengerPayloadType GetPayloadType()
    {
        return MessengerPayloadType.System;
    }

    protected override void Write(CborWriter writer)
    {
        writer.WriteTextString(Message);
    }

    public override string AsString()
    {
        return Message;
    }

    public override async Task Handle(string publisher, WebsocketClient client, ServerPacketHandler packetHandler)
    {
        var config = packetHandler.Config;
        await packetHandler.Bot.SendTextMessageAsync(config.GetTelegramChatId(), $"【系统 {publisher}】{AsString()}");
    }
}