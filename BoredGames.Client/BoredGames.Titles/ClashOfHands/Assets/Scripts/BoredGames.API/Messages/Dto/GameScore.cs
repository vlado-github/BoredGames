using System.Collections.Generic;

namespace Assets.Scripts.BoredGames.API
{
    [System.Serializable]
    public class GameScore
    {
        public int CurrentRound;
        public int RequiredNumberOfWins;
        public IList<PlayerScore> PlayerScores;
    }

    [System.Serializable]
    public class PlayerScore
    {
        public string PlayerId;
        public string PlayerNickName;
        public IList<RoundResult> RoundWins;
        public IList<RoundResult> RoundLosses;
        public IList<RoundResult> RoundDraws;
    }

    [System.Serializable]
    public class RoundResult
    {
        public int RoundNumber;
        public string PlayerMove;
    }
}