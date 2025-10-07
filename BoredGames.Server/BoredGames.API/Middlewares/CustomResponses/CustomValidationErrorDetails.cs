using System.Net;
using BoredGames.Common.Consts;
using FluentValidation.Results;

namespace BoredGames.API.Middlewares.CustomResponses
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
