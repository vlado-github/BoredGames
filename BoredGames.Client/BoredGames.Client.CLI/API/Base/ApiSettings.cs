using Microsoft.Extensions.Configuration;

namespace BoredGames.Client.CLI.API.Base;

public class ApiSettings
{
    private readonly IConfigurationSection _configSection;
    
    public ApiSettings(IConfigurationSection configSection)
    {
        _configSection = configSection;
        HeaderPlayerIdValue = Guid.NewGuid().ToString();
    }

    public string BaseUrl => _configSection[nameof(BaseUrl)];
    public string HeaderApiKeyName => "X-BORED-GAMES-API-KEY";
    public string HeaderApiKeyValue => Environment.GetEnvironmentVariable("BORED_GAMES_API_KEY");
    public string HeaderPlayerIdName => "X-BORED-GAMES-PLAYER-ID";
    public string HeaderPlayerIdValue { get; private set; }
}