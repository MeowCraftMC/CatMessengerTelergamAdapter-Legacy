using System.Formats.Cbor;

namespace CatMessenger.Telegram.Connector.Payloads;

public class SystemMessagePayload : MessengerPayloadBase
{
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
        return $"【系统】{RemoteName}: {Message}";
    }
    
    private string Message { get; }
    
    public SystemMessagePayload(string remoteName, string message) : base(remoteName)
    {
        Message = message;
    }

    public SystemMessagePayload(CborReader reader) : base(reader)
    {
        Message = reader.ReadTextString();
    }
}