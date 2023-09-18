using System.Formats.Cbor;
using Websocket.Client;

namespace CatMessenger.Telegram.Connector.Payloads;

public class PlayerDeathPayload : MessengerPayloadBase
{
    private string DeathMessage { get; }
    private string PlayerName { get; }
    private string KillerName { get; }
    private string ItemName { get; }
    
    public PlayerDeathPayload(string deathMessage, string playerName, string killerName, string itemName)
    {
        DeathMessage = deathMessage;
        PlayerName = playerName;
        KillerName = killerName;
        ItemName = itemName;
    }

    public PlayerDeathPayload(CborReader reader) : base(reader)
    {
        DeathMessage = reader.ReadTextString();
        PlayerName = reader.ReadTextString();
        KillerName = reader.ReadTextString();
        ItemName = reader.ReadTextString();
    }
    
    public override MessengerPayloadType GetPayloadType()
    {
        return MessengerPayloadType.PlayerDeath;
    }

    protected override void Write(CborWriter writer)
    {
        writer.WriteTextString(DeathMessage);
        writer.WriteTextString(PlayerName);
        writer.WriteTextString(KillerName);
        writer.WriteTextString(ItemName);
    }

    public override string AsString()
    {
        var pattern = DeathMessage.Replace("%1$s", "{0}")
            .Replace("%2$s", "{1}")
            .Replace("%3$s", "{2}");
        return string.Format(pattern, PlayerName, KillerName, ItemName);
    }
}