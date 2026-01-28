using Assets.Scripts.BoredGames.API.Responses;
using System;
using System.Text.Json.Serialization;
using UnityEngine;

[Serializable]
public class ApiTokenResponse : IResponse
{
    public string access_token;

    public int expires_in;

    [NonSerialized]
    public DateTime ExpiresAt;

    public bool IsValid()
    {
        return !string.IsNullOrEmpty(access_token) && DateTime.UtcNow < ExpiresAt;
    }
}