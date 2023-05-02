using BoredGames.Server.Common.Enums;
using BoredGames.Server.Domain.Games.Base;
using Orleans;

namespace BoredGames.Server.Domain.Games.Entities;

[GenerateSerializer]
public class GameDefinition
{
    [Id(0)]
    public Guid GameId { get; set; }
    [Id(1)]
    public GameState State { get; set; }
    [Id(2)]
    public int RequiredNumberOfPlayers { get; set; }
    [Id(3)]
    public int RequiredNumberOfWins { get; set; }
    [Id(4)]
    public string? Description { get; set; }
}