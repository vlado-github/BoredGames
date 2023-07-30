namespace BoredGames.Server.Domain.Games.Base;

public class GameConfigurationBase
{
    public int RequiredNumberOfPlayers { get; set; }
    public int RequiredNumberOfWins { get; set; }
    public int NumberOfRounds { get; set; }
    public string? Description { get; set; }
}