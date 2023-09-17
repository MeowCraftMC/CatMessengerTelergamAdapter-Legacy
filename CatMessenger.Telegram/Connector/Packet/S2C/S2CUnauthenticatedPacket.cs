using System.Formats.Cbor;
using Microsoft.Extensions.Logging;
using Websocket.Client;

namespace CatMessenger.Telegram.Connector.Packet.S2C;

public class S2CUnauthenticatedPacket : S2CPacket
{
    public override Task Handle(WebsocketClient client, ServerPacketHandler packetHandler, CborReader reader)
    {
        packetHandler.Logger.LogError("Unauthenticated, check your secret key?");
        
        return Task.CompletedTask;
    }
}