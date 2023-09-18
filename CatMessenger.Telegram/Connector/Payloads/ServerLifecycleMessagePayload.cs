using System.Formats.Cbor;

namespace CatMessenger.Telegram.Connector.Payloads;

public class ServerLifecycleMessagePayload : MessengerPayloadBase
{
    public override MessengerPayloadType GetPayloadType()
    {
        return MessengerPayloadType.ServerLifecycle;
    }

    protected override void Write(CborWriter writer)
    {
        writer.WriteBoolean(Launched);
    }

    public override string AsString()
    {
        var text = Launched ? "启动" : "关闭";
        return $"服务器 {RemoteName} {text}了";
    }
    
    private bool Launched { get; }
    
    public ServerLifecycleMessagePayload(string remoteName, bool launched) : base(remoteName)
    {
        Launched = launched;
    }

    public ServerLifecycleMessagePayload(CborReader reader) : base(reader)
    {
        Launched = reader.ReadBoolean();
    }
}