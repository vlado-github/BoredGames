using Assets.Scripts.BoredGames.API.Responses;

[System.Serializable]
public class ApiTokenResponse : IResponse
{
    public string access_token { get; set; }
}