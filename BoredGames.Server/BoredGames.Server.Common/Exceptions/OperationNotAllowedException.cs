namespace BoredGames.Server.Common.Exceptions;

public class OperationNotAllowedException : Exception
{
    public OperationNotAllowedException(string message)
        : base(message)
    {
    }
}