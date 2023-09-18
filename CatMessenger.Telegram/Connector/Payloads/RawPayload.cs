using System.Formats.Cbor;
using Telegram.Bot;
using Websocket.Client;

namespace CatMessenger.Telegram.Connector.Payloads;

public class RawPayload : MessengerPayloadBase
{
    private string Text { get; }
    
    public RawPayload(string text)
    {
        Text = text;
    }

    public RawPayload(CborReader reader) : base(reader)
    {
        Text = reader.ReadTextString();
    }

    public override MessengerPayloadType GetPayloadType()
    {
        return MessengerPayloadType.Raw;
    }

    protected override void Write(CborWriter writer)
    {
        writer.WriteTextString(Text);
    }

    public override string AsString()
    {
        return Text;
    }
}