namespace BoredGames.Client.CLI.API.Requests;

public class MakeMoveRequest
{
    public MakeMoveRequest(Guid gameId, string actionType)
    {
        GameId = gameId;
        ActionType = actionType;
    }
    
    public Guid GameId { get; private set; }
    public string ActionType { get; private set; }
}