using System;

namespace Assets.Scripts.BoredGames.API
{
    [System.Serializable]
    public class MakeMoveMessage
    {
        public string GameId;
        public string PlayerId;
        public string ActionType;
    }
}