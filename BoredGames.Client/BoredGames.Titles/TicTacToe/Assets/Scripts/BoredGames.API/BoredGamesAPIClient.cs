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
        private static ApiTokenResponse cachedToken; 

        public static BoredGamesAPIClient Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new BoredGamesAPIClient();
                    cachedToken = new ApiTokenResponse();
                }
                return _instance;
            }
        }

        public static IEnumerator GetToken(Action<ApiTokenResponse> onSuccess)
        {
            var url = new Uri(ApiConfig.BaseApiUrl, "/api/auth/token");

            using (UnityWebRequest request = UnityWebRequest.Get(url))
            {
                yield return request.SendWebRequest();

                if (request.result == UnityWebRequest.Result.ConnectionError 
                    || request.result == UnityWebRequest.Result.DataProcessingError
                    || request.result == UnityWebRequest.Result.ProtocolError)
                {
                    var errorMessage = $"[{ApiConfig.BaseApiUrl}] {request.uri} Request error: {request.error}]";
                    Debug.LogError(errorMessage);
                }
                else
                {
                    string jsonResp = request.downloadHandler.text;
                    var response = JsonUtility.FromJson<ApiTokenResponse>(jsonResp);
                    response.ExpiresAt = DateTime.UtcNow.AddSeconds(response.expires_in - 30);
                    cachedToken = response;
                    onSuccess(response);
                }
            }
        }

        private IEnumerator HandleRequest<T>(UnityWebRequest request, Action<T> onSuccess, ApiTokenResponse token) where T : IResponse
        {
            request.SetHeaders(token);

            // Send the request and wait for the response
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.ConnectionError 
                || request.result == UnityWebRequest.Result.DataProcessingError
                || request.result == UnityWebRequest.Result.ProtocolError)
            {
                var errorMessage = $"[{ApiConfig.BaseApiUrl}] {request.uri} Request error: {request.error}]";
                Debug.LogError(errorMessage);
            }
            else
            {
                var response = JsonUtility.FromJson<T>(request.downloadHandler.text);
                onSuccess(response);
            }

        }

        public IEnumerator GetTitles(Action<TitlesResponse> onSuccess)
        {
            ApiTokenResponse token = null;
            if (cachedToken.IsValid())
            {
                token = cachedToken;
            }
            else
            {
                yield return GetToken((t) => token = t);
            }

            var url = new Uri(ApiConfig.BaseApiUrl, "/api/game/titles");

            using (UnityWebRequest request = UnityWebRequest.Get(url))
            {
                yield return HandleRequest(request, onSuccess, token);
            }
        }

        public IEnumerator GetWinners(Action<WinnersResponse> onSuccess)
        {
            ApiTokenResponse token = null;
            if (cachedToken.IsValid())
            {
                token = cachedToken;
            }
            else
            {
                yield return GetToken((t) => token = t);
            }

            var url = new Uri(ApiConfig.BaseApiUrl, $"/api/game/{GameState.Instance.GameId}/winners");

            using (UnityWebRequest request = UnityWebRequest.Get(url))
            {
                yield return HandleRequest(request, onSuccess, token);
            }
        }

        public IEnumerator CreateGame(Action<CreateGameResponse> onSuccess)
        {
            ApiTokenResponse token = null;
            if (cachedToken.IsValid())
            {
                token = cachedToken;
            }
            else
            {
                yield return GetToken((t) => token = t);
            }

            var url = new Uri(ApiConfig.BaseApiUrl, "/api/game/create");

            var data = new CreateGameRequest
            {
                gameTitle = GameConfiguration.Instance.GameTitle,
                numberOfPlayers = GameConfiguration.Instance.NumberOfPlayers,
                requiredNumberOfConsecutiveWins = GameConfiguration.Instance.RequiredNumberOfConsecutiveWins,
                numberOfRounds = GameConfiguration.Instance.NumberOfRounds,
            };

            using (UnityWebRequest request = UnityWebRequest.Post(url, JsonUtility.ToJson(data), ApiConfig.DefaultContentType))
            {
                yield return HandleRequest(request, onSuccess, token);
            }
        }

        public IEnumerator JoinGame(Action<GameStateResponse> onSuccess)
        {
            ApiTokenResponse token = null;
            if (cachedToken.IsValid())
            {
                token = cachedToken;
            }
            else
            {
                yield return GetToken((t) => token = t);
            }

            var url = new Uri(ApiConfig.BaseApiUrl, "/api/game/join");
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
                    Debug.LogWarning($"JoinGame {data.gameId} {data.playerNickName} ");
                    yield return HandleRequest(request, onSuccess, token);
                }
            }
        }

        public IEnumerator GetGameState(Action<GameStateResponse> onSuccess)
        {
            ApiTokenResponse token = null;
            if (cachedToken.IsValid())
            {
                token = cachedToken;
            }
            else
            {
                yield return GetToken((t) => token = t);
            }

            var url = new Uri(ApiConfig.BaseApiUrl, $"/api/game/{GameState.Instance.GameId}/state");

            using (UnityWebRequest request = UnityWebRequest.Get(url))
            {
                yield return HandleRequest(request, onSuccess, token);
            }
        }

        public IEnumerator CreatePlayerSession(Action<PlayerDetailsResponse> onSuccess)
        {
            ApiTokenResponse token = null;
            if (cachedToken.IsValid())
            {
                token = cachedToken;
            }
            else
            {
                yield return GetToken((t) => token = t);
            }

            var url = new Uri(ApiConfig.BaseApiUrl, $"/api/player");
            var data = new CreatePlayerProfileRequest
            {
                nickname = GameState.Instance.PlayerName
            };

            using (UnityWebRequest request = UnityWebRequest.Post(url, JsonUtility.ToJson(data), ApiConfig.DefaultContentType))
            {
                yield return HandleRequest(request, onSuccess, token);
            }
        }

        public IEnumerator UpdatePlayerSessionDetails(Action<PlayerDetailsResponse> onSuccess)
        {
            ApiTokenResponse token = null;
            if (cachedToken.IsValid())
            {
                token = cachedToken;
            }
            else
            {
                yield return GetToken((t) => token = t);
            }

            var url = new Uri(ApiConfig.BaseApiUrl, $"/api/player");
            var data = new UpdatePlayerProfileRequest
            {
                nickname = GameState.Instance.PlayerName
            };

            using (UnityWebRequest request = UnityWebRequest.Put(url, JsonUtility.ToJson(data)))
            {
                yield return HandleRequest(request, onSuccess, token);
            }
        }

        public IEnumerator GetPlayerSessionDetails(Action<PlayerDetailsResponse> onSuccess)
        {
            ApiTokenResponse token = null;
            if (cachedToken.IsValid())
            {
                token = cachedToken;
            }
            else
            {
                yield return GetToken((t) => token = t);
            }

            var url = new Uri(ApiConfig.BaseApiUrl, $"/api/player/details");

            using (UnityWebRequest request = UnityWebRequest.Get(url))
            {
                yield return HandleRequest(request, onSuccess, token);
            }
        }
    }
}
