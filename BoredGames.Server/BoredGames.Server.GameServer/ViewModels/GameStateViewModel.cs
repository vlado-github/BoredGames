using BoredGames.Common.Enums;

namespace BoredGames.Server.GameServer.ViewModels;

[GenerateSerializer]
public class GameStateViewModel
{
    [Id(0)]
    public Guid GameId { get; set; }
    [Id(1)]
    public GameStatus GameStatus { get; set; }
    [Id(2)]
    public int RoundNumber { get; set; }
    [Id(3)]
    public RoundStatus RoundStatus { get; set; }
    [Id(4)]
    public int PlayersNumber { get; set; }
    [Id(5)]
    public GameScoreViewModel Score { get; set; } = new ();
}