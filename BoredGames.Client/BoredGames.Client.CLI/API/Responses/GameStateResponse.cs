namespace BoredGames.Client.CLI.API.Responses;

public class GameStateResponse
{
    public Guid Id { get; set; }
    public GameStateEnum State { get; set; }
}