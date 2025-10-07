using System.Net;
using BoredGames.Common.Consts;

namespace BoredGames.API.Middlewares.CustomResponses
{
    public class UnauthorizedErrorDetails : ErrorDetailsBase
    {
        public UnauthorizedErrorDetails(string message)
            : base(RfcConsts.RfcUnauthorizedType, (int)HttpStatusCode.Unauthorized, message)
        {

        }
    }
}
