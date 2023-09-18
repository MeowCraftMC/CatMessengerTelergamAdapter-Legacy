using System.Formats.Cbor;
using Telegram.Bot;
using Websocket.Client;

namespace CatMessenger.Telegram.Connector.Payloads;

public class ChatTextPayload : MessengerPayloadBase
{
    private string Sender { get; }
    private string Content { get; }
    
    public ChatTextPayload(string sender, string content)
    {
        Sender = sender;
        Content = content;
    }

    public ChatTextPayload(CborReader reader) : base(reader)
    {
        Sender = reader.ReadTextString();
        Content = reader.ReadTextString();
    }

    public override MessengerPayloadType GetPayloadType()
    {
        return MessengerPayloadType.ChatText;
    }

    protected override void Write(CborWriter writer)
    {
        writer.WriteTextString(Sender);
        writer.WriteTextString(Content);
    }

    public override string AsString()
    {
        return $"{Sender}: \n{Content}";
    }
}