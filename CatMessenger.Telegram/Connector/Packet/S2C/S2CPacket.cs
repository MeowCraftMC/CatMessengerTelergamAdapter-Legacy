﻿using System.Formats.Cbor;
using Websocket.Client;

namespace CatMessenger.Telegram.Connector.Packet.S2C;

public abstract class S2CPacket
{
    public abstract void Handle(WebsocketClient client, ServerPacketHandler packetHandler, CborReader reader);
}