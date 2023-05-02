using BoredGames.Server.Common.Enums;
using BoredGames.Server.Domain.Commands;
using BoredGames.Server.Domain.Games.Entities;
using BoredGames.Server.Domain.Games.RockPaperScissors;

namespace BoredGames.Server.Domain.Games.Base;

public static class GameRuleEngineFactory
{
    public static IGameRuleEngine GetInstance(CreateGameCommand command)
    {
        switch (command.Title)
        {
            case GameTitle.RockPaperScissors:
            {
                var ruleEngine = new RockPaperScissorsRuleEngine();
                ruleEngine.Setup(new RockPaperScissorsSettings(
                    requiredNumberOfPlayers: command.NumberOfPlayers,
                    requiredNumberOfWins: command.NumberOfWins,
                    description: command.Description));
                return ruleEngine;
            }
            default:
            {
                throw new NotImplementedException($"Game {command.Title} doesn't exist.");
            }
        }
    }
}