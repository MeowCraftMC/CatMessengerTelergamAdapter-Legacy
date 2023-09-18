using System.Formats.Cbor;
using Websocket.Client;

namespace CatMessenger.Telegram.Connector.Packets.S2C;

public class S2CSuccessfulPacket : S2CPacket
{
    public override Task Handle(WebsocketClient client, ServerPacketHandler packetHandler, CborReader reader)
    {
        return Task.CompletedTask;
    }
}