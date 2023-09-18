using System.Formats.Cbor;

namespace CatMessenger.Telegram.Connector.Payloads;

public abstract class MessengerPayloadBase
{
    public abstract MessengerPayloadType GetPayloadType();

    public byte[] ToBytes()
    {
        var writer = new CborWriter();
        writer.WriteStartArray(null);
        writer.WriteInt32((int)GetPayloadType());
        writer.WriteTextString(RemoteName);
        Write(writer);
        writer.WriteEndArray();
        return writer.Encode();
    }

    protected abstract void Write(CborWriter writer);
    
    public abstract string AsString();
    
    public string RemoteName { get; }

    protected MessengerPayloadBase(string remoteName)
    {
        RemoteName = remoteName;
    }
    
    protected MessengerPayloadBase(CborReader reader)
    {
        RemoteName = reader.ReadTextString();
    }

    public static MessengerPayloadBase FromBytes(byte[] bytes)
    {
        var reader = new CborReader(bytes);
        reader.ReadStartArray();
        var type = (MessengerPayloadType) reader.ReadInt32();

        MessengerPayloadBase payload = type switch
        {
            MessengerPayloadType.Raw => new RawMessagePayload(reader),
            MessengerPayloadType.System => new SystemMessagePayload(reader),
            MessengerPayloadType.PlayerOnline => new PlayerOnlineMessagePayload(reader),
            MessengerPayloadType.ServerLifecycle => new ServerLifecycleMessagePayload(reader),
            MessengerPayloadType.PlayerDeath => new PlayerDeathMessagePayload(reader),
            MessengerPayloadType.PlayerAchievement => new PlayerAdvancementMessagePayload(reader),
            _ => throw new ArgumentOutOfRangeException()
        };

        reader.ReadEndArray();
        return payload;
    }
}