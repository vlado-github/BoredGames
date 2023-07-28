using BoredGames.Server.Common.Enums;
using BoredGames.Server.Domain.Games.Dtos;
using BoredGames.Server.Domain.Games.RockPaperScissors;

namespace BoredGames.Server.Domain.Games.Base;

public static class GameRuleEngineFactory
{
    public static IGameRuleEngine GetInstance(GameDto dto)
    {
        switch (dto.Title)
        {
            case GameTitle.RockPaperScissors:
            {
                var ruleEngine = new RockPaperScissorsRuleEngine();
                ruleEngine.Setup(new RockPaperScissorsConfiguration(
                    requiredNumberOfPlayers: dto.NumberOfPlayers,
                    requiredNumberOfWins: dto.NumberOfWins,
                    description: dto.Description));
                return ruleEngine;
            }
            default:
            {
                throw new NotImplementedException($"Game {dto.Title} doesn't exist.");
            }
        }
    }
}