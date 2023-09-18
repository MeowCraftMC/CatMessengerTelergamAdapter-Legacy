using Telegram.Bot;

namespace CatMessenger.Telegram.Bot;

public static class TelegramBotClientExtension
{
    private static Dictionary<string, int> RequestedQueryOnline { get; } = new();
    private static Dictionary<(string server, string world), (TimeQueryType Type, int ReplyTo)> RequestedQueryTime { get; } = new();
    
    public static void QueryRequestOnline(this ITelegramBotClient client, int replyTo, string serverName)
    {
        RequestedQueryOnline.Add(serverName, replyTo);
    }
    
    public static int? QueryOnlineRequest(this ITelegramBotClient client, string serverName)
    {
        RequestedQueryOnline.TryGetValue(serverName, out var value);
        RequestedQueryOnline.Remove(serverName);
        return value;
    }
    
    public static void QueryRequestTime(this ITelegramBotClient client, int replyTo, string serverName, string worldName, TimeQueryType type)
    {
        RequestedQueryTime.Add((serverName, worldName), (type, replyTo));
    }
    
    public static (TimeQueryType Type, int ReplyTo)? QueryTimeRequest(this ITelegramBotClient client, string serverName, string worldName)
    {
        var key = (serverName, worldName);
        RequestedQueryTime.TryGetValue(key, out var value);
        RequestedQueryTime.Remove(key);
        return value;
    }
    
    public enum TimeQueryType
    {
        DayTime = 0,
        GameTime = 1,
        Day = 2
    }
}