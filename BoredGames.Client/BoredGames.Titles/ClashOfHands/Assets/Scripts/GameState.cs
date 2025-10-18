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

        public string Id { get; set; } = string.Empty;
        public GameStatus Status { get; set; } = GameStatus.WaitingForPlayer;
        public string PlayerName { get; set; }
    }
}