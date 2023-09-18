using System.Formats.Cbor;
using Websocket.Client;

namespace CatMessenger.Telegram.Connector.Payloads;

public class QueryOnlinePayload : MessengerPayloadBase
{
    private string ServerName { get; }
    
    public QueryOnlinePayload(string serverName)
    {
        ServerName = serverName;
    }

    public QueryOnlinePayload(CborReader reader)
    {
        ServerName = reader.ReadTextString();
    } 
    
    public override MessengerPayloadType GetPayloadType()
    {
        return MessengerPayloadType.QueryOnline;
    }

    protected override void Write(CborWriter writer)
    {
        writer.WriteTextString(ServerName);
    }

    public override string AsString()
    {
        return $"QueryOnline: {ServerName}";
    }

    public override Task Handle(string publisher, WebsocketClient client, ServerPacketHandler packetHandler)
    {
        // I am not a server, so do nothing.
        return Task.CompletedTask;
    }
}