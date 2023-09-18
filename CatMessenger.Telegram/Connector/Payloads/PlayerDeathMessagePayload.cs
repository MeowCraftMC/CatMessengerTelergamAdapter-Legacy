using System.Formats.Cbor;

namespace CatMessenger.Telegram.Connector.Payloads;

public class PlayerDeathMessagePayload : MessengerPayloadBase
{
    public override MessengerPayloadType GetPayloadType()
    {
        return MessengerPayloadType.PlayerDeath;
    }

    protected override void Write(CborWriter writer)
    {
        writer.WriteTextString(PlayerName);
        writer.WriteTextString(DeathMessage);
    }

    public override string AsString()
    {
        return $"{RemoteName}: \n{DeathMessage}";
    }
    
    private string PlayerName { get; }
    private string DeathMessage { get; }
    
    public PlayerDeathMessagePayload(string remoteName, string playerName, string deathMessage) : base(remoteName)
    {
        PlayerName = playerName;
        DeathMessage = deathMessage;
    }

    public PlayerDeathMessagePayload(CborReader reader) : base(reader)
    {
        PlayerName = reader.ReadTextString();
        DeathMessage = reader.ReadTextString();
    }
}