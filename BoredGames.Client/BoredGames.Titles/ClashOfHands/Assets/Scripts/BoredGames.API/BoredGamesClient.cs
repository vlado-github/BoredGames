using Assets.BoredGames.API;
using Assets.BoredGames.API.Responses;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace Assets.Scripts.BoredGames.API
{
    public class BoredGamesClient
    {
        private static BoredGamesClient _instance = null;
        private readonly ApiConfig _apiConfig;

        private BoredGamesClient()
        {
            _apiConfig = new ApiConfig();
        }

        public static BoredGamesClient GetInstance()
        {
            if (_instance == null)
            {
                _instance = new BoredGamesClient();
            }
            return _instance;
        }

        public IEnumerator GetTitles(Action<TitlesResponse> onSuccess)
        {
            var url = new Uri(_apiConfig.BaseUrl, "/api/game/titles");

            using (UnityWebRequest request = UnityWebRequest.Get(url))
            {
                request.SetAuth(_apiConfig);

                // Send the request and wait for the response
                yield return request.SendWebRequest();

                if (request.result == UnityWebRequest.Result.ConnectionError ||
                    request.result == UnityWebRequest.Result.ProtocolError)
                {
                    var errorMessage = $"[{_apiConfig.BaseUrl}] Request error: {request.error}";
                    Debug.LogError(errorMessage);
                }
                else
                {
                    var response = JsonUtility.FromJson<TitlesResponse>(request.downloadHandler.text);
                    onSuccess(response);
                }
            }
        }
    }
}
