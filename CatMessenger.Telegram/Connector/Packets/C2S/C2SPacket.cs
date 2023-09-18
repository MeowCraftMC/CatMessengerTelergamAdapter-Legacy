using System.Formats.Cbor;

namespace CatMessenger.Telegram.Connector.Packets.C2S;

public abstract class C2SPacket
{
    public byte[] WritePacket()
    {
        var writer = new CborWriter();
        writer.WriteStartArray(null);
        Write(writer);
        writer.WriteEndArray();
        return writer.Encode();
    }

    protected abstract void Write(CborWriter writer);
}