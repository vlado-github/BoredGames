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
            StartCoroutine(BoredGamesClient.Instance.GetPlayerDetails((response) =>
            {
                GameState.Instance.PlayerId = response.id;
            }));

            Dictionary<string, string> queryParams = GetQueryParams();

            if (queryParams.ContainsKey(GameInstanceIdParamKey))
            {
                string value = queryParams[GameInstanceIdParamKey];
                if (string.IsNullOrEmpty(value))
                {
                    return;
                }
                GameState.Instance.GameId = value;
                
                StartCoroutine(BoredGamesClient.Instance.GetGameState((response) => {
                    GameState.Instance.Status = (GameStatus)response.gameStatus;
                    GameState.Instance.CurrentRoundNumber = response.roundNumber;
                    GameState.Instance.CurrentRoundStatus = response.roundStatus;

                    SceneManager.LoadScene("GamePlayScene");
                }));
            }
            

            if (GameState.Instance.Status == GameStatus.InPlay)
            {
                SceneManager.LoadScene("GamePlayScene");
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