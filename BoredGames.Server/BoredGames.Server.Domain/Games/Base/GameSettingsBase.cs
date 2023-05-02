using Orleans;

namespace BoredGames.Server.Domain.Games.Base;

[GenerateSerializer]
public class GameSettingsBase
{
    [Id(0)]
    public int RequiredNumberOfPlayers { get; set; }
    [Id(1)]
    public int RequiredNumberOfWins { get; set; }
    [Id(2)]
    public string? Description { get; set; }
}