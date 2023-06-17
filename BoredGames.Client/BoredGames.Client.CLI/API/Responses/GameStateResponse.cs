namespace BoredGames.Client.CLI.API.Responses;

public class GameStateResponse
{
    public Guid GameId { get; set; }
    public GameStateEnum State { get; set; }
    public int RequiredNumberOfPlayers { get; set; }
    public int RequiredNumberOfWins { get; set; }
    public string? Description { get; set; }
}