using BoredGames.Server.Common.Enums;
using BoredGames.Server.Domain.Commands.Base;
using Orleans;

namespace BoredGames.Server.Domain.Commands;

[GenerateSerializer]
public class CreateGameCommand : CommandBase
{
    [Id(0)]
    public GameTitle Title { get; set; }
    [Id(1)]
    public int NumberOfPlayers { get; set; }
    [Id(2)]
    public int NumberOfWins { get; set; }
    [Id(3)]
    public string? Description { get; set; }
}