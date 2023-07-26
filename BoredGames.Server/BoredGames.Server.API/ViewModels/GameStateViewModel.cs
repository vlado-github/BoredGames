using BoredGames.Server.Common.Enums;

namespace BoredGames.Server.API.ViewModels;

public class GameStateViewModel
{
    public Guid Id { get; set; }
    public GameStatus GameStatus { get; set; }
    public int RoundNumber { get; set; }
    public RoundStatus RoundStatus { get; set; }
}