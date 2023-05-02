using BoredGames.Server.Common.Enums;
using BoredGames.Server.Domain.Games.Entities;

namespace BoredGames.Server.API.ViewModels;

public class GameDefinitionViewModel
{
    public Guid GameId { get; set; }
    public GameState State { get; set; }
    public int RequiredNumberOfPlayers { get; set; } 
    public int RequiredNumberOfWins { get; set; } 
    public string? Description { get; set; } 
}