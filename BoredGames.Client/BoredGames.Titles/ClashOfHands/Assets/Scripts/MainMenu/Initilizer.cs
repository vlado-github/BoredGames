using Assets.Scripts.BoredGames.API;
using Assets.Scripts.GamePlay;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.Menu
{
    public class Initilizer : MonoBehaviour
    {
        public static string GameInstanceIdParamKey = "gameInstanceId";

        // Use this for initialization
        private void Awake()
        {
            Dictionary<string, string> queryParams = GetQueryParams();

            if (queryParams.ContainsKey(GameInstanceIdParamKey))
            {
                string value = queryParams[GameInstanceIdParamKey];
                if (string.IsNullOrEmpty(value))
                {
                    return;
                }
                GameState.Instance.GameId = value;

                //StartCoroutine(BoredGamesClient.Instance.CreatePlayerProfile())
            }
        }

        private Dictionary<string, string> GetQueryParams()
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            string url = Application.absoluteURL;

            if (url.Contains("?"))
            {
                string queryString = url.Substring(url.IndexOf("?") + 1);
                string[] paramPairs = queryString.Split('&');

                foreach (string pair in paramPairs)
                {
                    string[] keyValue = pair.Split('=');
                    if (keyValue.Length == 2)
                    {
                        string key = UnityWebRequest.UnEscapeURL(keyValue[0]);
                        string value = UnityWebRequest.UnEscapeURL(keyValue[1]);
                        parameters[key] = value;
                    }
                }
            }
            return parameters;
        }
    }
}