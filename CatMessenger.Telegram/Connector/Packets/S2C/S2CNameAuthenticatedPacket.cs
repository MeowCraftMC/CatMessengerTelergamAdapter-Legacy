using System.Formats.Cbor;
using Microsoft.Extensions.Logging;
using Websocket.Client;

namespace CatMessenger.Telegram.Connector.Packets.S2C;

public class S2CNameAuthenticatedPacket : S2CPacket
{
    public override Task Handle(WebsocketClient client, ServerPacketHandler packetHandler, CborReader reader)
    {
        packetHandler.Logger.LogError("NameAuthenticated, please change the name of client!");

        return Task.CompletedTask;
    }
}