using System.Text.Json.Serialization;

namespace BoredGames.API.Models;

public class AuthToken
{
    [JsonPropertyName("access_token")]
    public string AccessToken { get; set; }
    
    [JsonPropertyName("expires_in")]
    public long ExpiresIn { get; set; }
}