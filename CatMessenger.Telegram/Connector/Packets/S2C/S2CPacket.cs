using System.Formats.Cbor;
using Websocket.Client;

namespace CatMessenger.Telegram.Connector.Packets.S2C;

public abstract class S2CPacket
{
    public abstract Task Handle(WebsocketClient client, ServerPacketHandler packetHandler, CborReader reader);
}