using System.Formats.Cbor;

namespace CatMessenger.Telegram.Connector.Packets.C2S;

public class C2SRegisterChannelPacket : C2SPacket
{
    protected override void Write(CborWriter writer)
    {
        writer.WriteTextString(ConnectorConstants.RequestRegister);
        writer.WriteTextString(ConnectorConstants.ChannelId);
        writer.WriteInt32((int)MessageDirection.All);
    }
}