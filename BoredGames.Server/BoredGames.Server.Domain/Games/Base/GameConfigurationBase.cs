using BoredGames.Server.Domain.Games.Enums;

namespace BoredGames.Server.Domain.Games.Base;

public class GameConfigurationBase
{
    public int RequiredNumberOfPlayers { get; set; }
    public int RequiredNumberOfWins { get; set; }
    public int NumberOfRounds { get; set; }
    public string? Description { get; set; }
    public GameMechanicsType GameMechanicsType { get; set; }
    public IDictionary<string, object> Assets { get; set; } = new Dictionary<string, object>();
}