using System.Net;
using BoredGames.Server.Common.Consts;

namespace BoredGames.Server.API.Middlewares.CustomResponses
{
    public class ServerErrorDetails : ErrorDetailsBase
    {
        public ServerErrorDetails(string message)
            : base(RfcConsts.RfcInternalServerType, (int)HttpStatusCode.InternalServerError, message)
        {

        }
    }
}
