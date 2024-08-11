namespace BoredGames.Server.Domain.Games.Entities;

public class GameDefinition
{
    public Guid GameId { get; set; }
    public int RequiredNumberOfPlayers { get; set; }
    public int RequiredNumberOfConsecutiveWins { get; set; }
    public string? Description { get; set; }
}