using System.Formats.Cbor;
using CatMessenger.Telegram.Connector.Payloads;
using Telegram.Bot;
using Websocket.Client;

namespace CatMessenger.Telegram.Connector.Packets.S2C;

public class S2CForwardPacket : S2CPacket
{
    public override async Task Handle(WebsocketClient client, ServerPacketHandler packetHandler, CborReader reader)
    {
        var bytes = reader.ReadByteString();
        var publisher = reader.ReadTextString();
        var channel = reader.ReadTextString();

        if (channel != ConnectorConstants.ChannelId)
        {
            return;
        }
        
        var payload = MessengerPayloadBase.FromBytes(bytes);

        var chatId = packetHandler.Config.GetTelegramChatId();
        await packetHandler.Bot.SendTextMessageAsync(chatId, payload.AsString());
    }
}