namespace BoredGames.Server.Common.Exceptions;

public class InvalidActionException : Exception
{
    public readonly string Action;
    
    public InvalidActionException(string action, string message)
        : base(message)
    {
        Action = action;
    }
}