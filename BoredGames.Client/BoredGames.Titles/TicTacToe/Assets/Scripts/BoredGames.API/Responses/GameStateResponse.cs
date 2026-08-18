using System.Collections.Generic;

namespace Assets.Scripts.BoredGames.API.Responses
{
    [System.Serializable]
    public class GameStateResponse : IResponse
    {
        public string gameId;
        public int gameStatus;
        public int roundNumber;
        public int roundStatus;
        public string currentPlayerTurn;
        public string nextPlayerTurn;
        public List<Move> moves;
        public List<Player> players;
    }
}