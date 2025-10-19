namespace Assets.Scripts.BoredGames.API.Requests
{
    [System.Serializable]
    public class CreateGameRequest
    {
        public int gameTitle;
        public int numberOfPlayers;
        public int requiredNumberOfConsecutiveWins;
        public int numberOfRounds;
        public string description = string.Empty;
    }
}