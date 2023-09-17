using Microsoft.Extensions.Configuration;
using Telegram.Bot;

namespace CatMessenger.Telegram;

public class ConfigAccessor
{
    private IConfiguration Config { get; }
    
    public ConfigAccessor(IConfiguration config)
    {
        Config = config;
    }

    public bool IsDebug()
    {
        return HasDevEnvironmentVariable() || Config.GetValue<bool>("Debug");
    }

    public static string GetDevEnvironmentVariable()
    {
        return Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT") ?? "Production";
    }

    private static bool HasDevEnvironmentVariable()
    {
        return GetDevEnvironmentVariable().Equals("Development", StringComparison.OrdinalIgnoreCase);
    }
    
    public string GetTelegramToken()
    {
        return Config.GetValue<string>("Telegram:Token") ?? "";
    }
    
    public bool IsTelegramProxyEnabled()
    {
        return Config.GetValue<bool>("Telegram:Proxy:Enabled");
    }

    public string GetTelegramProxyUrl()
    {
        return Config.GetValue<string>("Telegram:Proxy:Url") ?? "";
    }
    
    public string GetTelegramChatId()
    {
        return Config.GetValue<string>("Telegram:ChatId")!;
    }

    public string GetConnectorUrl()
    {
        return Config.GetValue<string>("Connector:Url")!;
    }

    public string GetName()
    {
        return Config.GetValue<string>("ClientName")!;
    }

    public string GetConnectorKey()
    {
        return Config.GetValue<string>("Connector:Key")!;
    }
}