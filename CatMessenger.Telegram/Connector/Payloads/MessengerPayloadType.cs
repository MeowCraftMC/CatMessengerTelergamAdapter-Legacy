namespace CatMessenger.Telegram.Connector.Payloads;

public enum MessengerPayloadType
{
    Raw = 0,
    ChatComponent = 1,
    ChatText = 2,
    System = 3,
    PlayerOnline = 4,
    ServerLifecycle = 5,
    PlayerDeath = 6,
    PlayerAdvancement = 7,
    QueryOnline = 8,
    QueryTime = 9,
    RunCommand = 10,    // Todo: qyl27: Do not implement it.
    QueryResultOnline = 11,
    QueryResultTime = 12,
    CommandResult = 13  // Todo: qyl27: Do not implement it.
}