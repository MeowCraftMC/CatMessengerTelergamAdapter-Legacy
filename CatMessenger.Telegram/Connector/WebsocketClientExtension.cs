using CatMessenger.Telegram.Connector.Packet;
using CatMessenger.Telegram.Connector.Packet.C2S;
using Websocket.Client;

namespace CatMessenger.Telegram.Connector;

public static class WebsocketClientExtension
{
    public static void Send(this WebsocketClient client, C2SPacket packet)
    {
        client.Send(packet.WritePacket());
    }
}