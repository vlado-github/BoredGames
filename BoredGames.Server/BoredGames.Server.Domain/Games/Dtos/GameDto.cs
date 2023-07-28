using BoredGames.Server.Common.Enums;

namespace BoredGames.Server.Domain.Games.Dtos;

public class GameDto
{
    public GameTitle Title { get; set; }
    public int NumberOfPlayers { get; set; }
    public int NumberOfWins { get; set; }
    public string? Description { get; set; }
    public string? PlayerNickName { get; set; }
}