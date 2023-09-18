using System.Formats.Cbor;
using CatMessenger.Telegram.Connector.Packets.S2C;
using Microsoft.Extensions.Logging;
using Telegram.Bot;
using Websocket.Client;

namespace CatMessenger.Telegram.Connector;

public class ServerPacketHandler
{
    public ILogger<ServerPacketHandler> Logger { get; }
    public ConfigAccessor Config { get; }
    public ITelegramBotClient Bot { get; }

    private Dictionary<string, S2CPacket> Handlers { get; } = new();
    
    public ServerPacketHandler(ILogger<ServerPacketHandler> logger, ConfigAccessor config, ITelegramBotClient bot)
    {
        Logger = logger;
        Config = config;
        Bot = bot;
        
        Handlers.Add(ConnectorConstants.ResponseSuccessful, new S2CSuccessfulPacket());
        Handlers.Add(ConnectorConstants.ResponseUnauthenticated, new S2CUnauthenticatedPacket());
        Handlers.Add(ConnectorConstants.ResponseNameAuthenticated, new S2CUnauthenticatedPacket());
        Handlers.Add(ConnectorConstants.ResponseForward, new S2CForwardPacket());
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