using System;

namespace Assets.Scripts.BoredGames.API.Responses
{
    [System.Serializable]
    public class CreateGameResponse : IResponse
    {
        public string gameId;
        public int requiredNumberOfPlayers;
        public int requiredNumberOfWins;
        public int numberOfRounds;
    }
}