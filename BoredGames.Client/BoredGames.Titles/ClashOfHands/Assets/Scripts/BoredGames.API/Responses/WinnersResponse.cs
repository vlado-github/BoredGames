namespace Assets.Scripts.BoredGames.API.Responses
{
    [System.Serializable]
    public class PlayerDetails : IResponse
    {
        public string id;
        public string nickName;
    }

    [System.Serializable]
    public class WinnersResponse : IResponse
    {
        public PlayerDetails[] winners;
    }
}
