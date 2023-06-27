namespace BoredGames.Client.CLI.API.Responses;

public class GameDefinitionResponse
{
    public Guid GameId { get; set; }
    public GameStatusEnum Status { get; set; }
    public int RequiredNumberOfPlayers { get; set; }
    public int RequiredNumberOfWins { get; set; }
    public string? Description { get; set; }
}