using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.Networking;

namespace Assets.BoredGames.API
{
    static class AuthHelper
    {
        public static void SetAuth(this UnityWebRequest request, ApiConfig apiConfig)
        {
            request.SetRequestHeader(apiConfig.HeaderKey, apiConfig.ApiKey);
        }
    }
}
