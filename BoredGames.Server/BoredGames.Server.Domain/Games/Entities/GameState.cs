using BoredGames.Server.Common.Enums;
using Orleans;

namespace BoredGames.Server.Domain.Games.Entities;

[GenerateSerializer]
public class GameState
{
    [Id(0)]
    public Guid GameId { get; set; }
    [Id(1)]
    public GameStatus GameStatus { get; set; }
    [Id(2)]
    public RoundStatus RoundStatus { get; set; }
    [Id(3)]
    public int RoundNumber { get; set; }
}