using System;
using UnityEngine;
using UnityEngine.SceneManagement;

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

        public string GameId { get; set; }
        public GameStatus Status { get; set; } = GameStatus.AwaitingPlayers;
        public string PlayerId { get; set; } = null;
        public string PlayerName { get; set; }
        public int CurrentRoundNumber { get; set; }
        public RoundStatus CurrentRoundStatus { get; set; }

        public bool IsGameCreated => !string.IsNullOrEmpty(GameId);
    }
}