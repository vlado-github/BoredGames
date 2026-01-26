using Assets.Scripts.BoredGames.API.Responses;
using Assets.Scripts.GamePlay;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

namespace Assets.BoredGames.API
{
    static class AuthHelper
    {
        public static IEnumerator GetToken(Action<ApiTokenResponse> onSuccess)
        {
            var url = new Uri(ApiConfig.BaseApiUrl, "/api/auth/token");

            using (UnityWebRequest request = UnityWebRequest.Get(url))
            {
                yield return request.SendWebRequest();

                if (request.result == UnityWebRequest.Result.ConnectionError ||
                    request.result == UnityWebRequest.Result.ProtocolError)
                {
                    var errorMessage = $"[{ApiConfig.BaseApiUrl}] {request.uri} Request error: {request.error}]";
                    Debug.LogError(errorMessage);
                }
                else
                {
                    var response = JsonUtility.FromJson<ApiTokenResponse>(request.downloadHandler.text);
                    onSuccess(response);
                }
            }
        }

        public static void SetHeaders(this UnityWebRequest request)
        {
            request.SetRequestHeader("Accept", "application/json");
            request.SetRequestHeader("Content-Type", "application/json");
            GetToken((token) =>
            {
                request.SetRequestHeader("Authorization", $"Bearer {token}");
            });
            if (!string.IsNullOrEmpty(GameState.Instance.PlayerId))
            {
                request.SetRequestHeader(ApiConfig.HeaderPlayerKey, GameState.Instance.PlayerId.ToString());
            }
        }
    }
}
