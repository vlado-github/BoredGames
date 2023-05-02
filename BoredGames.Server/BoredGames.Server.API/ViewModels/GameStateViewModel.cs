using BoredGames.Server.Common.Enums;

namespace BoredGames.Server.API.ViewModels;

public class GameStateViewModel
{
    public Guid Id { get; set; }
    public GameState State { get; set; }
}