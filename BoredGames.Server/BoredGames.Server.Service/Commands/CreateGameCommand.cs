using BoredGames.Server.Common.Enums;
using BoredGames.Server.Service.Commands.Base;
using Orleans;

namespace BoredGames.Server.Service.Commands;

[GenerateSerializer]
public class CreateGameCommand : CommandBase
{
    [Id(0)]
    public GameTitle Title { get; set; }
    [Id(1)]
    public int NumberOfPlayers { get; set; }
    [Id(2)]
    public int RequiredNumberOfWins { get; set; }
    [Id(3)]
    public int NumberOfRounds { get; set; }
    [Id(4)]
    public string? Description { get; set; }
    [Id(5)]
    public string? PlayerNickName { get; set; }
    [Id(6)]
    public Guid PlayerId { get; set; }
}