using System.Formats.Cbor;
using Websocket.Client;

namespace CatMessenger.Telegram.Connector.Packet.S2C;

public class S2CSuccessfulPacket : S2CPacket
{
    public override Task Handle(WebsocketClient client, ServerPacketHandler packetHandler, CborReader reader)
    {
        return Task.CompletedTask;
    }
}