namespace CatMessenger.Telegram.Connector;

public interface IConnectorClientService
{
    void Start();
    
    void Stop();
    
    void SendChatMessage(string message);
}