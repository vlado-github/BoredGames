using FluentValidation.Results;
using System.Net;
using BoredGames.Server.Common.Consts;

namespace BoredGames.Server.API.Middlewares.CustomResponses
{
    public class CustomValidationErrorDetails : ErrorDetailsBase
    {
        public CustomValidationErrorDetails(string message)
            : base(RfcConsts.RfcBadRequestType, (int)HttpStatusCode.BadRequest, message)
        {
        }

        public CustomValidationErrorDetails(string message, IEnumerable<ValidationFailure> errors)
            : base(RfcConsts.RfcBadRequestType, (int)HttpStatusCode.BadRequest, message)
        {
            Errors = new Dictionary<string, string[]>();
            foreach (var error in errors)
            {
                Errors.Add(error.PropertyName, 
                    errors.Where(x => x.PropertyName == error.PropertyName).Select(x => x.ErrorMessage).ToArray());
            }
        }

        public IDictionary<string, string[]> Errors { get; private set; }

    }
}
