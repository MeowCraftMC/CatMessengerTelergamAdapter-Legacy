using System.Formats.Cbor;
using System.Net.WebSockets;
using System.Reactive.Linq;
using CatMessenger.Telegram.Connector.Packets.C2S;
using CatMessenger.Telegram.Connector.Payloads;
using Microsoft.Extensions.Logging;
using Websocket.Client;

namespace CatMessenger.Telegram.Connector;

public class ConnectorClientService : IConnectorClientService
{
    private ILogger<ConnectorClientService> Logger { get; }
    private ConfigAccessor Config { get; }
    private ServerPacketHandler PacketHandler { get; }
    
    private WebsocketClient WebsocketClient { get; }
    
    public ConnectorClientService(ILogger<ConnectorClientService> logger, ConfigAccessor config, ServerPacketHandler packetHandler)
    {
        Logger = logger;
        Config = config;
        PacketHandler = packetHandler;

        WebsocketClient = new WebsocketClient(new Uri(Config.GetConnectorUrl()));

        WebsocketClient.ReconnectTimeout = null;
        WebsocketClient.ReconnectionHappened.Subscribe(info =>
        {
            Logger.LogInformation("Connected! Type: {Type}", info.Type);
            WebsocketClient.Send(new C2SAuthenticatePacket(Config.GetConnectorKey(), Config.GetName()));
            WebsocketClient.Send(new C2SRegisterChannelPacket());
        });

        WebsocketClient.MessageReceived.Where(msg => msg.MessageType == WebSocketMessageType.Binary)
            .Subscribe(message =>
            {
                try
                {
                    PacketHandler.ReadPacket(WebsocketClient, message.Binary!);
                }
                catch (CborContentException ex)
                {
                    Logger.LogWarning(ex, "Bad packet!");
                }
                catch (InvalidOperationException ex)
                {
                    Logger.LogWarning(ex, "Malformed packet!");
                }
                catch (ArgumentOutOfRangeException ex)
                {
                    Logger.LogWarning(ex, "Argument out of range!");
                }
            });
    }
    
    public void Start()
    {
        WebsocketClient.Start();
        Logger.LogInformation("Connector connecting...");
    }

    public void Stop()
    {
        WebsocketClient.Stop(WebSocketCloseStatus.NormalClosure, "");
        Logger.LogInformation("Connector disconnecting...");
    }

    public void SendChatMessage(MessengerPayloadBase payload)
    {
        WebsocketClient.Send(new C2SPublishPacket(payload));
    }
}