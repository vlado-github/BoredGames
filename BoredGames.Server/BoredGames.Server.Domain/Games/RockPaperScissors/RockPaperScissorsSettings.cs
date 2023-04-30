using BoredGames.Server.Domain.Games.Base;
using BoredGames.Server.Domain.Games.Entities;

namespace BoredGames.Server.Domain.Games.RockPaperScissors;

public class RockPaperScissorsSettings : GameSettingsBase
{
    public static readonly int MinimumRequiredNumberOfWins = 1;
    public static readonly int MinimumRequiredNumberOfPlayers = 2;
    public static readonly RockPaperScissorsSettings Default = new RockPaperScissorsSettings();

    public RockPaperScissorsSettings(int? requiredNumberOfPlayers = null, 
        int? requiredNumberOfWins = null)
    {
        RequiredNumberOfPlayers = requiredNumberOfPlayers ?? MinimumRequiredNumberOfPlayers;
        RequiredNumberOfWins = requiredNumberOfWins ?? MinimumRequiredNumberOfWins;
    }
}