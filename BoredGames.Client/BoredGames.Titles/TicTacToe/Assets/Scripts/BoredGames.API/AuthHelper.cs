using Assets.Scripts.GamePlay;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

namespace Assets.BoredGames.API
{
    static class AuthHelper
    {
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
