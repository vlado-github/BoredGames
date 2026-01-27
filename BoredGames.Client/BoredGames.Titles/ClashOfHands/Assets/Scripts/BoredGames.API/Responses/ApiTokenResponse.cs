using Assets.Scripts.BoredGames.API.Responses;
using System;
using System.Text.Json.Serialization;

[System.Serializable]
public class ApiTokenResponse : IResponse
{
    [JsonPropertyName("access_token")]
    public string access_token { get; set; } = string.Empty;

    [JsonPropertyName("expires_in")]
    public long? expires_in { get; set; } = null;

    public bool IsValid()
    {
        var isSet = !string.IsNullOrEmpty(access_token) && expires_in != null;
        if (isSet)
        {
            return DateTime.UtcNow < DateTime.UtcNow.AddSeconds(expires_in.Value - 30) ;
        }
        return false;
    }
}