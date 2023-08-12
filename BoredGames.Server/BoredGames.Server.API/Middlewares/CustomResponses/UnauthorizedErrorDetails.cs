using System.Net;
using BoredGames.Server.Common.Consts;

namespace BoredGames.Server.API.Middlewares.CustomResponses
{
    public class UnauthorizedErrorDetails : ErrorDetailsBase
    {
        public UnauthorizedErrorDetails(string message)
            : base(RfcConsts.RfcUnauthorizedType, (int)HttpStatusCode.Unauthorized, message)
        {

        }
    }
}
