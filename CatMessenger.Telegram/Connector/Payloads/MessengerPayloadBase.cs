using System.Formats.Cbor;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;
using Websocket.Client;

namespace CatMessenger.Telegram.Connector.Payloads;

public abstract class MessengerPayloadBase
{
    protected MessengerPayloadBase()
    {
    }
    
    protected MessengerPayloadBase(CborReader reader)
    {
    }
    
    public abstract MessengerPayloadType GetPayloadType();

    public byte[] ToBytes()
    {
        var writer = new CborWriter();
        writer.WriteStartArray(null);
        writer.WriteInt32((int)GetPayloadType());
        Write(writer);
        writer.WriteEndArray();
        return writer.Encode();
    }

    protected abstract void Write(CborWriter writer);
    
    public abstract string AsString();

    public virtual async Task Handle(string publisher, WebsocketClient client, ServerPacketHandler packetHandler)
    {
        var config = packetHandler.Config;
        await packetHandler.Bot.SendTextMessageAsync(config.GetTelegramChatId(), 
            $"【{publisher}】{AsString()}", parseMode: ParseMode.Html);
    }

    public static MessengerPayloadBase FromBytes(byte[] bytes)
    {
        var reader = new CborReader(bytes);
        reader.ReadStartArray();
        var type = (MessengerPayloadType) reader.ReadInt32();

        MessengerPayloadBase payload = type switch
        {
            MessengerPayloadType.Raw => new RawPayload(reader),
            MessengerPayloadType.ChatComponent => new ChatComponentPayload(reader),
            MessengerPayloadType.ChatText => new ChatTextPayload(reader),
            MessengerPayloadType.System => new SystemPayload(reader),
            MessengerPayloadType.PlayerOnline => new PlayerOnlinePayload(reader),
            MessengerPayloadType.ServerLifecycle => new ServerLifecyclePayload(reader),
            MessengerPayloadType.PlayerDeath => new PlayerDeathPayload(reader),
            MessengerPayloadType.PlayerAdvancement => new PlayerAdvancementPayload(reader),
            MessengerPayloadType.QueryOnline or
            MessengerPayloadType.QueryTime or
            MessengerPayloadType.QueryResultOnline or
            MessengerPayloadType.QueryResultTime or
            MessengerPayloadType.CommandResult or
            MessengerPayloadType.RunCommand or
            _ => throw new ArgumentOutOfRangeException()
        };

        reader.ReadEndArray();
        return payload;
    }
}