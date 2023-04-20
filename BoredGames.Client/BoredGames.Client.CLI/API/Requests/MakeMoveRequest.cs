namespace BoredGames.Client.CLI.API.Requests;

public class MakeMoveRequest
{
    public Guid GameId { get; set; }
    public string ActionType { get; set; }
}