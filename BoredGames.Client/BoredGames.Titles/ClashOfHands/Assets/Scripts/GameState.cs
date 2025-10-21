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

        public string GameId { get; set; } = "439c00b7-453b-44ef-a27c-152a66e6785e";
        public GameStatus Status { get; set; } = GameStatus.AwaitingPlayers;
        public string PlayerId { get; set; }
        public string PlayerName { get; set; }
        public int CurrentRoundNumber { get; set; }
        public int CurrentRoundStatus { get; set; }

        public void SetGameInstanceId(string gameId)
        {
            GameId = gameId;
            
            SceneManager.LoadScene("GamePlayScene");
            
            Debug.Log("Received gameInstanceId: " + gameId);
        }
    }
}