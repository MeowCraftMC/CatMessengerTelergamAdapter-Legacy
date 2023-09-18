using System.Formats.Cbor;

namespace CatMessenger.Telegram.Connector.Payloads;

public class PlayerAdvancementPayload : MessengerPayloadBase
{
    private string PlayerName { get; }
    private AdvancementType Type { get; }
    private string Name { get; }
    private string Description { get; }
    
    public PlayerAdvancementPayload(string playerName, AdvancementType type, string name, string description)
    {
        PlayerName = playerName;
        Type = type;
        Name = name;
        Description = description;
    }

    public PlayerAdvancementPayload(CborReader reader) : base(reader)
    {
        PlayerName = reader.ReadTextString();

        var type = reader.ReadInt32();
        Type = !Enum.IsDefined(typeof(AdvancementType), type) ? AdvancementType.Advancement : (AdvancementType) type;
        
        Name = reader.ReadTextString();
        Description = reader.ReadTextString();
    }
    
    public override MessengerPayloadType GetPayloadType()
    {
        return MessengerPayloadType.PlayerAdvancement;
    }

    protected override void Write(CborWriter writer)
    {
        writer.WriteTextString(PlayerName);
        writer.WriteInt32((int)Type);
        writer.WriteTextString(Name);
        writer.WriteTextString(Description);
    }

    private static string AdvancementTypeToString(AdvancementType advancementType)
    {
        return advancementType switch
        {
            AdvancementType.Advancement => "进度",
            AdvancementType.Goal => "目标",
            AdvancementType.Challenge => "挑战",
            _ => throw new ArgumentOutOfRangeException(nameof(advancementType), advancementType, null)
        };
    } 
    
    public enum AdvancementType
    {
        Advancement = 0,
        Goal = 1,
        Challenge = 2
    }

    public override string AsString()
    {
        return $"<b>{PlayerName}</b> 完成了{AdvancementTypeToString(Type)} <b>[{Name}]</b>\n{Description}";
    }
}