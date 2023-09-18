namespace CatMessenger.Telegram.Connector.Payloads;

public enum MessengerPayloadType
{
    Raw = 0,
    System = 1,
    PlayerOnline = 2,
    ServerLifecycle = 3,
    PlayerDeath = 4,
    PlayerAchievement = 5,
    
    // Todo: qyl27: Not implemented.
    QueryOnline = 6,
    QueryTime = 7,
    RunCommand = 8
}