using BoredGames.Server.Common.Enums;

namespace BoredGames.Server.API.Models;

public class GameViewModel
{
    public Guid Id { get; set; }
    public GameState State { get; set; }
}