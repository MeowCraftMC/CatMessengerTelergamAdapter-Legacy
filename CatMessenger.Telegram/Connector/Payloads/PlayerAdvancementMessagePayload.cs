using System.Formats.Cbor;

namespace CatMessenger.Telegram.Connector.Payloads;

public class PlayerAdvancementMessagePayload : MessengerPayloadBase
{
    public override MessengerPayloadType GetPayloadType()
    {
        return MessengerPayloadType.PlayerDeath;
    }

    protected override void Write(CborWriter writer)
    {
        writer.WriteTextString(PlayerName);
        writer.WriteTextString(Message);
        writer.WriteTextString(Description);
    }

    public override string AsString()
    {
        return $"{RemoteName}: \n{PlayerName} 取得了成就 {Message}\n{Description}";
    }
    
    private string PlayerName { get; }
    private string Message { get; }
    private string Description { get; }
    
    public PlayerAdvancementMessagePayload(string remoteName, string playerName, string message, string description) : base(remoteName)
    {
        PlayerName = playerName;
        Message = message;
        Description = description;
    }

    public PlayerAdvancementMessagePayload(CborReader reader) : base(reader)
    {
        PlayerName = reader.ReadTextString();
        Message = reader.ReadTextString();
        Description = reader.ReadTextString();
    }
}