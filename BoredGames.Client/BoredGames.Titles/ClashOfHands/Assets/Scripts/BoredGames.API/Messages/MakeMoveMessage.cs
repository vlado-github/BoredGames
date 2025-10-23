using System;

namespace Assets.Scripts.BoredGames.API
{
    [System.Serializable]
    public class MakeMoveMessage
    {
        public string gameId;
        public string playerId;
        public string actionType;
    }
}