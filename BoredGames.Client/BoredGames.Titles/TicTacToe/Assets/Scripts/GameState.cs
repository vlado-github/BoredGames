using Assets.Scripts;
using Assets.Scripts.BoredGames.API;
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;

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
        public string CurrentPlayerTurn { get; set; }
        public string[] PlayersTurnOrder { get; set; }
        public int PreviousRoundNumber { get; set; }
        public bool IsPreviousRoundCompleted => GameState.Instance.Score.HasRoundResult(PreviousRoundNumber);
        public IList<int> RoundResultDisplayCompleted { get; private set; } = new List<int>();
        public IList<Move> Moves = new List<Move>();
        public IList<Player> Players = new List<Player>();

        public GameScore Score { get; set; }

        public bool IsGameCreated => !string.IsNullOrEmpty(GameId);
        public bool IsPlayerSet => !string.IsNullOrEmpty(PlayerId);

        public bool IsRoundResultDisplayCompleted()
        {
            return RoundResultDisplayCompleted.Any(x => x == PreviousRoundNumber);
        }

        public bool IsPlayerTurn()
        {
            return !string.IsNullOrEmpty(CurrentPlayerTurn) && CurrentPlayerTurn == PlayerId;
        }

        public string GetActionType()
        {
            var index = Array.IndexOf(PlayersTurnOrder, GameState.Instance.PlayerId);
            if (index == 0)
            {
                return "x";
            }
            return "o";
        }
    }
}