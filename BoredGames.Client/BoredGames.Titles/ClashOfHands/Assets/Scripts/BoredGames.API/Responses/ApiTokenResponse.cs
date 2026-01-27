using Assets.Scripts.BoredGames.API.Responses;
using System;
using System.Text.Json.Serialization;

[Serializable]
public class ApiTokenResponse : IResponse
{
    public string access_token;

    public long? expires_in;

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