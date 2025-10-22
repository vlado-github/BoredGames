using BoredGames.Common.Enums;

namespace BoredGames.Server.Domain.Games.Entities;

public class GameState
{
    public Guid GameId { get; set; }
    public GameStatus GameStatus { get; set; }
    public RoundStatus RoundStatus { get; set; }
    public int RoundNumber { get; set; }
    public int PlayersNumber { get; set; }
}