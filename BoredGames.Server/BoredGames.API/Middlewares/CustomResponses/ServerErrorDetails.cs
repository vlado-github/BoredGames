using System.Net;
using BoredGames.Common.Consts;

namespace BoredGames.API.Middlewares.CustomResponses
{
    public class ServerErrorDetails : ErrorDetailsBase
    {
        public ServerErrorDetails(string message)
            : base(RfcConsts.RfcInternalServerType, (int)HttpStatusCode.InternalServerError, message)
        {

        }
    }
}
