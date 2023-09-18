using System.Formats.Cbor;
using Websocket.Client;

namespace CatMessenger.Telegram.Connector.Payloads;

public class QueryTimePayload : MessengerPayloadBase
{
    private string ServerName { get; }
    private string WorldName { get; }
    
    public QueryTimePayload(string serverName, string worldName)
    {
        ServerName = serverName;
        WorldName = worldName;
    }

    public QueryTimePayload(CborReader reader)
    {
        ServerName = reader.ReadTextString();
        WorldName = reader.ReadTextString();
    } 
    
    public override MessengerPayloadType GetPayloadType()
    {
        return MessengerPayloadType.QueryTime;
    }

    protected override void Write(CborWriter writer)
    {
        writer.WriteTextString(ServerName);
        writer.WriteTextString(WorldName);
    }

    public override string AsString()
    {
        return $"QueryTime: {ServerName}/{WorldName}";
    }

    public override Task Handle(string publisher, WebsocketClient client, ServerPacketHandler packetHandler)
    {
        // I am not a server, so do nothing.
        return Task.CompletedTask;
    }
}