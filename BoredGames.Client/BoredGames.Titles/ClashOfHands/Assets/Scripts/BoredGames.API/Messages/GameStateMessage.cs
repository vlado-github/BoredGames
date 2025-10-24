using System;
using System.Collections.Generic;

namespace Assets.Scripts.BoredGames.API
{
    [System.Serializable]
    public class GameStateMessage
    {
        public string GameId;
        public GameStatus GameStatus;
        public int RoundNumber;
        public RoundStatus RoundStatus;
        public int PlayersNumber;
        public GameScore Score;
    }
}