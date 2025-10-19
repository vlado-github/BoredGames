using Assets.Scripts.GamePlay;
using UnityEngine.Networking;

namespace Assets.BoredGames.API
{
    static class AuthHelper
    {
        public static void SetHeaders(this UnityWebRequest request)
        {
            request.SetRequestHeader("Accept", "application/json");
            request.SetRequestHeader("Content-Type", "application/json");
            request.SetRequestHeader(ApiConfig.HeaderKey, ApiConfig.ApiKey);
            if (!string.IsNullOrEmpty(GameState.Instance.PlayerId))
            {
                request.SetRequestHeader(ApiConfig.HeaderPlayerKey, GameState.Instance.PlayerId);
            }
        }
    }
}
