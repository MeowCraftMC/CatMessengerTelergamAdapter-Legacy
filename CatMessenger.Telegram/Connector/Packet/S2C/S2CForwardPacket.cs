using System.Formats.Cbor;
using Telegram.Bot;
using Websocket.Client;

namespace CatMessenger.Telegram.Connector.Packet.S2C;

public class S2CForwardPacket : S2CPacket
{
    public override async Task Handle(WebsocketClient client, ServerPacketHandler packetHandler, CborReader reader)
    {
        var chatId = packetHandler.Config.GetTelegramChatId();
        
        await packetHandler.Bot.SendTextMessageAsync(chatId, "");
    }
}