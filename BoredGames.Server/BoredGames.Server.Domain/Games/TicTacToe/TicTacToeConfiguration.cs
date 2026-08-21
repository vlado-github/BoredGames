using System.Collections.Generic;
using BoredGames.Server.Domain.Games.Base;

namespace BoredGames.Server.Domain.Games.TicTacToe;

public class TicTacToeConfiguration : GameConfigurationBase
{
    public static readonly int MinimumRequiredNumberOfWins = 1;
    public static readonly int MinimumRequiredNumberOfRounds = 1;
    public static readonly int MinimumRequiredNumberOfPlayers = 2;
    public static readonly TicTacToeConfiguration Default = new();

    public TicTacToeConfiguration(int? requiredNumberOfPlayers = null, 
        int? requiredNumberOfWins = null, int? numberOfRounds = null, string? description = null)
    {
        RequiredNumberOfPlayers = requiredNumberOfPlayers ?? MinimumRequiredNumberOfPlayers;
        RequiredNumberOfWins = requiredNumberOfWins ?? MinimumRequiredNumberOfWins;
        NumberOfRounds = numberOfRounds ?? MinimumRequiredNumberOfRounds;
        Description = description ?? string.Empty;
        GameSystemType = Enums.GameSystemType.TurnBase;
        Assets = new Dictionary<string, object>()
        {
            { 
                "actions", 
                new []
                {
                    TicTacToeRuleEngine.ExAction,
                    TicTacToeRuleEngine.OxAction
                } 
            }
        };
    }
}