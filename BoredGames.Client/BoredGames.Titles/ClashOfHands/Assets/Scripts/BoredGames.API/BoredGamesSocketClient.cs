using Assets.BoredGames.API;
using Assets.Scripts.GamePlay;
using System;
using System.Threading.Tasks;
using UnityEngine;
using WebSocketSharp;

namespace Assets.Scripts.BoredGames.API
{
    public class BoredGamesSocketClient
    {
        private static SignalR signalR = new SignalR();
        private static bool isConnected = false;

        public static void SetupConnection()
        {
            if (isConnected)
            {
                return;
            }

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
                Log($"OnGameStateReceived: {payload}");
                var data = JsonUtility.FromJson<GameStateMessage>(payload);

                var previousStatus = GameState.Instance.Status;

                GameState.Instance.Status = data.GameStatus;
                GameState.Instance.CurrentRoundStatus = data.RoundStatus;
                GameState.Instance.CurrentRoundNumber = data.RoundNumber;

                if (previousStatus != GameState.Instance.Status)
                {
                    GameManager.Instance.CheckGameStatus();
                }
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
                signalR.Invoke("JoinGameGroup", GameState.Instance.GameId, GameState.Instance.PlayerName);
            };

            signalR.ConnectionClosed += (object sender, ConnectionEventArgs e) =>
            {
                // Log the disconnected ID
                Log($"Disconnected: {e.ConnectionId}");
            };

            signalR.Connect();
        }

        public static void MakeMove(string gameId, MakeMoveMessage command)
        {
            signalR.Invoke("MakeMove", gameId, JsonUtility.ToJson(command));
        }

        public static void CloseConnection()
        {
            signalR.Stop();
        }

        public static bool IsConnected()
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