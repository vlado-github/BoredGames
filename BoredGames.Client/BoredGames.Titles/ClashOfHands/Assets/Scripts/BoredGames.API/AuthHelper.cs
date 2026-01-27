using Assets.Scripts.GamePlay;
using System;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

namespace Assets.BoredGames.API
{
    static class AuthHelper
    {

        private static ApiTokenResponse cachedToken = new ApiTokenResponse();


        public static async Task<ApiTokenResponse> GetToken()
        {
            if (cachedToken.IsValid())
            {
                return cachedToken;
            }
            else
            {
                var url = new Uri(ApiConfig.BaseApiUrl, "/api/auth/token");

                using (UnityWebRequest request = UnityWebRequest.Get(url))
                {
                    await request.SendWebRequest();

                    if (request.result == UnityWebRequest.Result.ConnectionError ||
                        request.result == UnityWebRequest.Result.ProtocolError)
                    {
                        var errorMessage = $"[{ApiConfig.BaseApiUrl}] {request.uri} Request error: {request.error}]";
                        Debug.LogError(errorMessage);
                        throw new Exception(errorMessage);
                    }
                    else
                    {
                        string jsonResp = request.downloadHandler.text;
                        Debug.Log(jsonResp);
                        cachedToken = JsonUtility.FromJson<ApiTokenResponse>(jsonResp);
                        Debug.Log(cachedToken.access_token);
                        Debug.Log(cachedToken.expires_in);
                        return cachedToken;
                    }
                }
            }
        }

        public static void SetHeaders(this UnityWebRequest request, ApiTokenResponse token)
        {
            request.SetRequestHeader("Accept", "application/json");
            request.SetRequestHeader("Content-Type", "application/json");
            request.SetRequestHeader("Authorization", $"Bearer {token.access_token}");
            if (!string.IsNullOrEmpty(GameState.Instance.PlayerId))
            {
                request.SetRequestHeader(ApiConfig.HeaderPlayerKey, GameState.Instance.PlayerId.ToString());
            }
        }
    }
}
