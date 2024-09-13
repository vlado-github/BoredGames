using BoredGames.Server.Common.Enums;

namespace BoredGames.Server.Domain.Games.Dtos;

public class GameDto
{
    public GameTitle Title { get; set; }
    public int NumberOfPlayers { get; set; }
    public int RequiredNumberOfWins { get; set; }
    public int NumberOfRounds { get; set; }
    public string? Description { get; set; }
    public string? PlayerNickName { get; set; }

    public static GameDto Default => new GameDto()
    {
        Title = GameTitle.RockPaperScissors,
        NumberOfPlayers = 2,
        RequiredNumberOfWins = 1,
        NumberOfRounds = 10
    };
}