using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

namespace Assets.Scripts.GamePlay
{
    public sealed class GameConfiguration
    {
        private static GameConfiguration _instance = null;

        public static GameConfiguration Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new GameConfiguration();
                }
                return _instance;
            }
        }

        public int GameTitle { get; private set; } = 0;
        public int NumberOfPlayers { get; private set; } = 2;
        public int RequiredNumberOfConsecutiveWins { get; set; } = 1;
        public int NumberOfRounds { get; private set; } = 100;

        public string BaseUrl = "https://boredgames.lol";
    }
}