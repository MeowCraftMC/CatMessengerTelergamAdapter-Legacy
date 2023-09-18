using CatMessenger.Telegram.Connector.Payloads;

namespace CatMessenger.Telegram.Connector;

public interface IConnectorClientService
{
    void Start();
    
    void Stop();
    
    void SendChatMessage(MessengerPayloadBase payload);
}