using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

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

        public string GameId { get; set; } = string.Empty;
        public GameStatus Status { get; set; } = GameStatus.WaitingForPlayer;
        public string PlayerId { get; set; }
        public string PlayerName { get; set; }
    }
}