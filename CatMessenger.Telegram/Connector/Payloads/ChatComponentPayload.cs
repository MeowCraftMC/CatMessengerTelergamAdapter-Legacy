using System.Formats.Cbor;
using System.Net.Mime;
using CatMessenger.Telegram.Utilities.MessageComponent;
using Newtonsoft.Json;
using Telegram.Bot;
using Websocket.Client;

namespace CatMessenger.Telegram.Connector.Payloads;

public class ChatComponentPayload : MessengerPayloadBase
{
    private string Component { get; }

    public ChatComponentPayload(string component)
    {
        Component = component;
    }

    public ChatComponentPayload(CborReader reader) : base(reader)
    {
        Component = reader.ReadTextString();
    }
    
    public override MessengerPayloadType GetPayloadType()
    {
        return MessengerPayloadType.ChatComponent;
    }

    protected override void Write(CborWriter writer)
    {
        writer.WriteTextString(Component);
    }

    public override string AsString()
    {
        var message = JsonConvert.DeserializeObject<ChatMessage>(Component);
        return message!.AsPlainText();
    }
}