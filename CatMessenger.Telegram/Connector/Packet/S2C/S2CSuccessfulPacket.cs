using System.Formats.Cbor;
using Websocket.Client;

namespace CatMessenger.Telegram.Connector.Packet.S2C;

public class S2CSuccessfulPacket : S2CPacket
{
    public override void Handle(WebsocketClient client, ServerPacketHandler packetHandler, CborReader reader)
    {
    }
}