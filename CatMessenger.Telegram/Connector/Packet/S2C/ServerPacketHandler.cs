using System.Formats.Cbor;
using Microsoft.Extensions.Logging;
using Websocket.Client;

namespace CatMessenger.Telegram.Connector.Packet.S2C;

public class ServerPacketHandler
{
    public ILogger<ServerPacketHandler> Logger { get; }
    public ConfigAccessor Config { get; }

    private Dictionary<string, S2CPacket> Handlers { get; } = new();
    
    public ServerPacketHandler(ILogger<ServerPacketHandler> logger, ConfigAccessor config)
    {
        Logger = logger;
        Config = config;
        
        Handlers.Add(ConnectorConstants.ResponseSuccessful, new S2CSuccessfulPacket());
        Handlers.Add(ConnectorConstants.ResponseUnauthenticated, new S2CUnauthenticatedPacket());
        Handlers.Add(ConnectorConstants.ResponseNameAuthenticated, new S2CUnauthenticatedPacket());
    }
    
    public void ReadPacket(WebsocketClient client, byte[] data)
    {
        var reader = new CborReader(data);
        reader.ReadStartArray();
        var operation = reader.ReadTextString();
        if (Handlers.TryGetValue(operation, out var packet))
        {
            try
            {
                packet.Handle(client, this, reader);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Exception while processing {Operation}!", operation);
            }
        }
        else
        {
            Logger.LogError("We have no handler for {Operation}!", operation);
        }
        reader.ReadEndArray();
    }
}