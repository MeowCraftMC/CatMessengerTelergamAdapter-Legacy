using CatMessenger.Telegram.Connector.Packets.C2S;
using Websocket.Client;

namespace CatMessenger.Telegram.Connector;

public static class WebsocketClientExtension
{
    public static void Send(this WebsocketClient client, C2SPacket packet)
    {
        client.Send(packet.WritePacket());
    }
}