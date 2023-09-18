using System.Formats.Cbor;

namespace CatMessenger.Telegram.Connector.Packets.C2S;

public class C2SAuthenticatePacket : C2SPacket
{
    private string SecretKey { get; }
    private string ClientName { get; }
    
    public C2SAuthenticatePacket(string secretKey, string clientName)
    {
        SecretKey = secretKey;
        ClientName = clientName;
    }
    
    protected override void Write(CborWriter writer)
    {
        writer.WriteTextString(ConnectorConstants.RequestAuthenticate);
        writer.WriteTextString(SecretKey);
        writer.WriteTextString(ClientName);
    }
}