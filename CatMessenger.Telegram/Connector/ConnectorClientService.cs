using System.Net.WebSockets;
using System.Reactive.Linq;
using CatMessenger.Telegram.Connector.Packet.C2S;
using CatMessenger.Telegram.Connector.Packet.S2C;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Websocket.Client;

namespace CatMessenger.Telegram.Connector;

public class ConnectorClientService : IHostedService
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

        WebsocketClient.ReconnectTimeout = TimeSpan.FromSeconds(15);
        WebsocketClient.ReconnectionHappened.Subscribe(info =>
        {
            Logger.LogInformation("Connected! Type: {Type}", info.Type);
            WebsocketClient.Send(new C2SAuthenticatePacket(Config.GetConnectorKey(), Config.GetName()));
            WebsocketClient.Send(new C2SRegisterChannelPacket());
        });

        WebsocketClient.MessageReceived.Where(msg => msg.MessageType == WebSocketMessageType.Binary)
            .Subscribe(message =>
            {
                PacketHandler.ReadPacket(WebsocketClient, message.Binary!);
            });
    }
    
    public Task StartAsync(CancellationToken cancellationToken)
    {
        WebsocketClient.Start();
        Logger.LogInformation("Connector connected!");
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        WebsocketClient.Stop(WebSocketCloseStatus.NormalClosure, "");
        Logger.LogInformation("Connector disconnected!");
        return Task.CompletedTask;
    }

    public void SendMessage()
    {
        // WebsocketClient.Send();
        
        // Todo: qyl27: Payload format.
    }
}