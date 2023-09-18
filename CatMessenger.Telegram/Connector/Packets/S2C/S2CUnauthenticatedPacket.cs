using System.Formats.Cbor;
using Microsoft.Extensions.Logging;
using Websocket.Client;

namespace CatMessenger.Telegram.Connector.Packets.S2C;

public class S2CUnauthenticatedPacket : S2CPacket
{
    public override Task Handle(WebsocketClient client, ServerPacketHandler packetHandler, CborReader reader)
    {
        packetHandler.Logger.LogError("Unauthenticated, check your secret key?");
        
        return Task.CompletedTask;
    }
}