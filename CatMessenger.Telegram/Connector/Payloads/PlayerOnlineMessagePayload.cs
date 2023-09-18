using System.Formats.Cbor;

namespace CatMessenger.Telegram.Connector.Payloads;

public class PlayerOnlineMessagePayload : MessengerPayloadBase
{
    public override MessengerPayloadType GetPayloadType()
    {
        return MessengerPayloadType.PlayerOnline;
    }

    protected override void Write(CborWriter writer)
    {
        writer.WriteBoolean(Leave);
        writer.WriteTextString(PlayerName);
    }

    public override string AsString()
    {
        var text = Leave ? "离开" : "加入";
        return $"{PlayerName} {text}了 {RemoteName} 服务器";
    }
    
    private bool Leave { get; }
    private string PlayerName { get; }
    
    public PlayerOnlineMessagePayload(string remoteName, bool isLeave, string playerName) : base(remoteName)
    {
        Leave = isLeave;
        PlayerName = playerName;
    }

    public PlayerOnlineMessagePayload(CborReader reader) : base(reader)
    {
        Leave = reader.ReadBoolean();
        PlayerName = reader.ReadTextString();
    }
}