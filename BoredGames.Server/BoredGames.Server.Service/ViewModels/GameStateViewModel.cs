using BoredGames.Server.Common.Enums;
using Orleans;

namespace BoredGames.Server.Service.ViewModels;

[GenerateSerializer]
public class GameStateViewModel
{
    [Id(0)]
    public Guid Id { get; set; }
    [Id(1)]
    public GameStatus GameStatus { get; set; }
    [Id(2)]
    public int RoundNumber { get; set; }
    [Id(3)]
    public RoundStatus RoundStatus { get; set; }
}