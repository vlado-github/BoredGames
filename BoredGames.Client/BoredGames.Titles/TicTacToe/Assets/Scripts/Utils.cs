using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace Assets.Scripts
{
    public static class Utils
    {
        public static Dictionary<string, string> GetQueryParams()
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