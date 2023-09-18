using System.Formats.Cbor;
using CatMessenger.Telegram.Utilities.MessageComponent;
using Newtonsoft.Json;

namespace CatMessenger.Telegram.Connector.Payloads;

public class RawMessagePayload : MessengerPayloadBase
{
    public override MessengerPayloadType GetPayloadType()
    {
        return MessengerPayloadType.Raw;
    }

    protected override void Write(CborWriter writer)
    {
        writer.WriteTextString(RawComponent);
    }

    public override string AsString()
    {
        var message = JsonConvert.DeserializeObject<ChatMessage>(RawComponent);
        return message!.AsPlainText();
    }

    private string RawComponent { get; }

    public RawMessagePayload(string remoteName, string rawComponent) : base(remoteName)
    {
        RawComponent = rawComponent;
    }

    public RawMessagePayload(CborReader reader) : base(reader)
    {
        RawComponent = reader.ReadTextString();
    }
}