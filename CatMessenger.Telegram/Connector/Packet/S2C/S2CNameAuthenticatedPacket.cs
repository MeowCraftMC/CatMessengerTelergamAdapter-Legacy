using System.Formats.Cbor;
using Microsoft.Extensions.Logging;
using Websocket.Client;

namespace CatMessenger.Telegram.Connector.Packet.S2C;

public class S2CNameAuthenticatedPacket : S2CPacket
{
    public override void Handle(WebsocketClient client, ServerPacketHandler packetHandler, CborReader reader)
    {
        packetHandler.Logger.LogError("NameAuthenticated, please change the name of client!");
    }
}