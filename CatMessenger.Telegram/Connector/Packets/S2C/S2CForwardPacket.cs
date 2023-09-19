using System.Formats.Cbor;
using CatMessenger.Telegram.Connector.Payloads;
using Websocket.Client;

namespace CatMessenger.Telegram.Connector.Packets.S2C;

public class S2CForwardPacket : S2CPacket
{
    public override async Task Handle(WebsocketClient client, ServerPacketHandler packetHandler, CborReader reader)
    {
        var publisher = reader.ReadTextString();
        var channel = reader.ReadTextString();
        var bytes = reader.ReadByteString();

        if (channel != ConnectorConstants.ChannelId)
        {
            return;
        }
        
        var payload = MessengerPayloadBase.FromBytes(bytes);
        await payload.Handle(publisher, client, packetHandler);

        // var chatId = packetHandler.Config.GetTelegramChatId();
        // await packetHandler.Bot.SendTextMessageAsync(chatId, payload.AsString());
    }
}