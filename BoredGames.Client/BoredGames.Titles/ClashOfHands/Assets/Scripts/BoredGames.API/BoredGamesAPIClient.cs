using Assets.BoredGames.API;
using Assets.Scripts.BoredGames.API.Requests;
using Assets.Scripts.BoredGames.API.Responses;
using Assets.Scripts.GamePlay;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

namespace Assets.Scripts.BoredGames.API
{
    public class BoredGamesAPIClient
    {
        private static BoredGamesAPIClient _instance = null;

        public static BoredGamesAPIClient Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new BoredGamesAPIClient();
                }
                return _instance;
            }
        }

        private IEnumerator HandleRequest<T>(UnityWebRequest request, Action<T> onSuccess) where T : IResponse
        {
            request.SetHeaders();

            // Send the request and wait for the response
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.ConnectionError ||
                request.result == UnityWebRequest.Result.ProtocolError)
            {
                var errorMessage = $"[{ApiConfig.BaseUrl}] {request.uri} Request error: {request.error}]";
                Debug.LogError(errorMessage);
            }
            else
            {
                var response = JsonUtility.FromJson<T>(request.downloadHandler.text);
                Debug.Log($"[server] {request.uri} response: "+ JsonUtility.ToJson(response));
                onSuccess(response);
            }
        }

        public IEnumerator GetTitles(Action<TitlesResponse> onSuccess)
        {
            var url = new Uri(ApiConfig.BaseUrl, "/api/game/titles");

            using (UnityWebRequest request = UnityWebRequest.Get(url))
            {
                yield return HandleRequest(request, onSuccess);
            }
        }

        public IEnumerator CreateGame(Action<CreateGameResponse> onSuccess)
        {
            var url = new Uri(ApiConfig.BaseUrl, "/api/game/create");

            var data = new CreateGameRequest
            {
                gameTitle = GameConfiguration.Instance.GameTitle,
                numberOfPlayers = GameConfiguration.Instance.NumberOfPlayers,
                requiredNumberOfConsecutiveWins = GameConfiguration.Instance.RequiredNumberOfConsecutiveWins,
                numberOfRounds = GameConfiguration.Instance.NumberOfRounds,
            };

            using (UnityWebRequest request = UnityWebRequest.Post(url, JsonUtility.ToJson(data), ApiConfig.DefaultContentType))
            {
                yield return HandleRequest(request, onSuccess);
            }
        }

        public IEnumerator JoinGame(Action<GameStateResponse> onSuccess)
        {
            var url = new Uri(ApiConfig.BaseUrl, "/api/game/join");
            var isValid = true;

            if (!GameState.Instance.IsGameCreated)
            {
                Debug.LogWarning($"{nameof(GameState.Instance.GameId)} has no value set.");
                isValid = false;
            }
            if (string.IsNullOrEmpty(GameState.Instance.PlayerId))
            {
                Debug.LogWarning($"{nameof(GameState.Instance.PlayerId)} has no value set.");
                isValid = false;
            }
            if (string.IsNullOrEmpty(GameState.Instance.PlayerName))
            {
                Debug.LogWarning($"{nameof(GameState.Instance.PlayerName)} has no value set.");
                isValid = false;
            }

            if (isValid)
            {
                var data = new JoinGameRequest
                {
                    gameId = GameState.Instance.GameId,
                    playerNickName = GameState.Instance.PlayerName
                };

                using (UnityWebRequest request = UnityWebRequest.Put(url, JsonUtility.ToJson(data)))
                {
                    yield return HandleRequest(request, onSuccess);
                }
            }
        }

        public IEnumerator GetGameState(Action<GameStateResponse> onSuccess)
        {
            var url = new Uri(ApiConfig.BaseUrl, $"/api/game/{GameState.Instance.GameId}/state");

            using (UnityWebRequest request = UnityWebRequest.Get(url))
            {
                yield return HandleRequest(request, onSuccess);
            }
        }

        public IEnumerator CreatePlayerSession(Action<PlayerDetailsResponse> onSuccess)
        {
            var url = new Uri(ApiConfig.BaseUrl, $"/api/player");
            var data = new CreatePlayerProfileRequest
            {
                nickname = GameState.Instance.PlayerName
            };

            using (UnityWebRequest request = UnityWebRequest.Post(url, JsonUtility.ToJson(data), ApiConfig.DefaultContentType))
            {
                yield return HandleRequest(request, onSuccess);
            }
        }

        public IEnumerator UpdatePlayerSessionDetails(Action<PlayerDetailsResponse> onSuccess)
        {
            var url = new Uri(ApiConfig.BaseUrl, $"/api/player");
            var data = new UpdatePlayerProfileRequest
            {
                nickname = GameState.Instance.PlayerName
            };

            using (UnityWebRequest request = UnityWebRequest.Put(url, JsonUtility.ToJson(data)))
            {
                yield return HandleRequest(request, onSuccess);
            }
        }

        public IEnumerator GetPlayerSessionDetails(Action<PlayerDetailsResponse> onSuccess)
        {
            var url = new Uri(ApiConfig.BaseUrl, $"/api/player/details");

            using (UnityWebRequest request = UnityWebRequest.Get(url))
            {
                yield return HandleRequest(request, onSuccess);
            }
        }
    }
}
