using BoredGames.Server.Domain.Games.Base;
using BoredGames.Server.Domain.Games.Entities;

namespace BoredGames.Server.Domain.Games.RockPaperScissors;

public class RockPaperScissorsConfiguration : GameConfigurationBase
{
    public static readonly int MinimumRequiredNumberOfWins = 1;
    public static readonly int MinimumRequiredNumberOfRounds = 1;
    public static readonly int MinimumRequiredNumberOfPlayers = 2;
    public static readonly RockPaperScissorsConfiguration Default = new RockPaperScissorsConfiguration();

    public RockPaperScissorsConfiguration(int? requiredNumberOfPlayers = null, 
        int? requiredNumberOfWins = null, int? numberOfRounds = null, string? description = null)
    {
        RequiredNumberOfPlayers = requiredNumberOfPlayers ?? MinimumRequiredNumberOfPlayers;
        RequiredNumberOfWins = requiredNumberOfWins ?? MinimumRequiredNumberOfWins;
        NumberOfRounds = numberOfRounds ?? MinimumRequiredNumberOfRounds;
        Description = description ?? string.Empty;
    }
}