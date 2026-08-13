using Assets.Scripts.GamePlay;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.BoredGames.API
{
    [System.Serializable]
    public class GameScore
    {
        public int LastRound;
        public int RequiredNumberOfWins;
        public PlayerScore[] PlayerScores;

        public IList<PlayerRoundScoreResult>  GetRoundResult(int roundNumber)
        {
            var result = new List<PlayerRoundScoreResult>();
            if (PlayerScores == null || PlayerScores.Length == 0)
            {
                return result;
            }
            foreach (var playerScore in PlayerScores)
            {
                var playerResult = new RoundScoreResult();
                var playerRoundWin = playerScore.RoundWins.FirstOrDefault(x => x.RoundNumber == roundNumber);
                if (playerRoundWin == null)
                {
                    var playerRoundLoss = playerScore.RoundLosses.FirstOrDefault(x => x.RoundNumber == roundNumber);
                    if (playerRoundLoss == null)
                    {
                        var playerRoundDraw = playerScore.RoundDraws.FirstOrDefault(x => x.RoundNumber == roundNumber);
                        if (playerRoundDraw != null)
                        {
                            playerResult.Result = RoundScoreResultEnum.Draw;
                            playerResult.SelectedCardTag = playerRoundDraw.PlayerMove.ToLower();
                        }
                    }
                    else
                    {
                        playerResult.Result = RoundScoreResultEnum.Loss;
                        playerResult.SelectedCardTag = playerRoundLoss.PlayerMove.ToLower();
                    }
                }
                else
                {
                    playerResult.Result = RoundScoreResultEnum.Win;
                    playerResult.SelectedCardTag = playerRoundWin.PlayerMove.ToLower();
                }

                result.Add(new PlayerRoundScoreResult { 
                    PlayerId = playerScore.PlayerId, 
                    RoundResult = playerResult 
                });
            }

            return result;
        }

        public bool HasRoundResult(int roundNumber) => GetRoundResult(roundNumber).Count == GameConfiguration.Instance.NumberOfPlayers;
    }

    [System.Serializable]
    public class PlayerScore
    {
        public string PlayerId;
        public string PlayerNickName;
        public RoundResult[] RoundWins;
        public RoundResult[] RoundLosses;
        public RoundResult[] RoundDraws;
    }

    [System.Serializable]
    public class RoundResult
    {
        public int RoundNumber;
        public string PlayerMove;
    }
}