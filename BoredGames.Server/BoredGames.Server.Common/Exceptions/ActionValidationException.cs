namespace BoredGames.Server.Common.Exceptions;

public class ActionValidationException : Exception
{
    public ActionValidationException(string message)
        : base(message)
    {
    }
}