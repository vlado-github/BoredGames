namespace BoredGames.Server.GameServer.ViewModels;

[GenerateSerializer]
public class GameDefinitionViewModel
{
    [Id(0)]
    public Guid GameId { get; set; }
    [Id(1)]
    public int RequiredNumberOfPlayers { get; set; } 
    [Id(2)]
    public int RequiredNumberOfWins { get; set; } 
    [Id(3)]
    public int NumberOfRounds { get; set; } 
    [Id(4)]
    public string? Description { get; set; }
    [Id(5)]
    public IDictionary<string, object> Assets { get; set; } = new Dictionary<string, object>();
}