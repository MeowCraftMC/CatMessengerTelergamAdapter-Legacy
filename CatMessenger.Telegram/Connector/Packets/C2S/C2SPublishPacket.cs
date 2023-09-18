using System.Formats.Cbor;
using CatMessenger.Telegram.Connector.Payloads;

namespace CatMessenger.Telegram.Connector.Packets.C2S;

public class C2SPublishPacket : C2SPacket
{
    private MessengerPayloadBase Payload { get; }
    
    public C2SPublishPacket(MessengerPayloadBase payload)
    {
        Payload = payload;
    }
    
    protected override void Write(CborWriter writer)
    {
        writer.WriteTextString(ConnectorConstants.RequestPublish);
        writer.WriteTextString(ConnectorConstants.ChannelId);
        writer.WriteByteString(Payload.ToBytes());
    }
}