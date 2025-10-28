using Assets.Scripts;
using Assets.Scripts.BoredGames.API;
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Assets.Scripts.GamePlay
{
    public sealed class GameState
    {
        private static GameState _instance = null;

        public static GameState Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new GameState();
                }
                return _instance;
            }
        }

        public string GameId { get; set; } = null;
        public GameStatus Status { get; set; } = GameStatus.AwaitingPlayers;
        public string PlayerId { get; set; } = null;
        public string PlayerName { get; set; }
        public int CurrentRoundNumber { get; set; }
        public RoundStatus CurrentRoundStatus { get; set; }
        public string CurrentRoundSelectedPlayerCard { get; set; } = null;
        public string CurrentRoundSelectedOpponentCard { get; set; } = null;

        public int PreviousRoundNumber { get; set; }
        public bool IsPreviousRoundCompleted => PreviousRoundNumber < GameState.Instance.CurrentRoundNumber && GameState.Instance.Score.HasRoundResult(PreviousRoundNumber);
        public IList<int> RoundResultDisplayCompleted { get; private set; } = new List<int>();

        public GameScore Score { get; set; }

        public bool IsGameCreated => !string.IsNullOrEmpty(GameId);
        public bool IsPlayerSet => !string.IsNullOrEmpty(PlayerId);

        public bool IsRoundResultDisplayCompleted()
        {
            return RoundResultDisplayCompleted.Any(x => x == PreviousRoundNumber);
        }

        public void CompleteRoundResultDisplay()
        {
            CurrentRoundSelectedPlayerCard = null;
            CurrentRoundSelectedOpponentCard = null;
            RoundResultDisplayCompleted.Add(PreviousRoundNumber);
        }
    }
}