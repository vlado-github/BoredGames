using Assets.BoredGames.API;
using Assets.Scripts.GamePlay;
using System;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.BoredGames.API
{
    public class BoredGamesSocketClient
    {
        private static BoredGamesSocketClient _instance = null;

        public static BoredGamesSocketClient Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new BoredGamesSocketClient();
                }
                return _instance;
            }
        }

        private bool isConnected = false;
        private SignalR signalR = null;

        public void SetupConnection()
        {
            if (isConnected && signalR != null)
            {
                return;
            }

            signalR = new SignalR();

            // Initialize SignalR
            var socketHubUrl = new Uri(ApiConfig.BaseUrl, ApiConfig.SocketHub);
            signalR.Init($"{socketHubUrl}");

            // Handler callback
            signalR.On("OnPlayerJoined", (string payload) =>
            {
                Log($"OnPlayerJoined: {payload}");
            }); 

            signalR.On("OnGameStateReceived", (string payload) =>
            {
                Log($">>> OnGameStateReceived: {payload}");
                var data = JsonUtility.FromJson<GameStateMessage>(payload);
                Log($"<<< OnGameStateReceived: {JsonUtility.ToJson(data)}");
                GameState.Instance.PreviousRoundNumber = GameState.Instance.CurrentRoundNumber;
                GameState.Instance.Status = data.GameStatus;
                GameState.Instance.CurrentRoundStatus = data.RoundStatus;
                GameState.Instance.CurrentRoundNumber = data.RoundNumber;
                GameState.Instance.Score = data.Score;
            });

            // Connection callback
            signalR.ConnectionStarted += (object sender, ConnectionEventArgs e) =>
            {
                // Log the connected ID
                Log($"Connected: {e.ConnectionId}");
                isConnected = true;

                if (!GameState.Instance.IsGameCreated)
                {
                    LogError("Game instance is not created.");
                }

                // Send payload to hub as JSON
                Log($"JoinGameGroup: GameId={GameState.Instance.GameId} Player={GameState.Instance.PlayerName}");
                signalR.Invoke("JoinGameGroup", GameState.Instance.GameId, GameState.Instance.PlayerName);
            };

            signalR.ConnectionClosed += (object sender, ConnectionEventArgs e) =>
            {
                // Log the disconnected ID
                Log($"Disconnected: {e.ConnectionId}");
            };

            signalR.Connect();
        }

        public void MakeMove(MakeMoveMessage command)
        {
            signalR.Invoke("MakeMove", JsonUtility.ToJson(command));
        }

        public void CloseConnection()
        {
            signalR.Stop();
        }

        public bool IsConnected()
        {
            return isConnected;
        }

        private static void Log(string message)
        {
            Debug.Log($"[socket]: {message}");
        }

        private static void LogError(string message)
        {
            Debug.LogError($"[socket]: {message}");
        }
    }
}