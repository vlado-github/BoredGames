using BoredGames.Common.Enums;
using BoredGames.Server.Domain.Games.Dtos;
using BoredGames.Server.Domain.Games.RockPaperScissors;
using BoredGames.Server.Domain.Games.TicTacToe;

namespace BoredGames.Server.Domain.Games.Base;

public static class GameRuleEngineFactory
{
    public static IGameRuleEngine GetInstance(GameDto dto)
    {
        switch (dto.Title)
        {
            case GameTitle.ClashOfHands:
            {
                var ruleEngine = new RockPaperScissorsRuleEngine();
                ruleEngine.Setup(new RockPaperScissorsConfiguration(
                    requiredNumberOfPlayers: dto.NumberOfPlayers,
                    numberOfRounds: dto.NumberOfRounds,
                    requiredNumberOfWins: dto.RequiredNumberOfWins,
                    description: dto.EndOfGameMessage));
                ruleEngine.Initialize();
                return ruleEngine;
            }
            case GameTitle.TicTacToe:
            {
                var ruleEngine = new TicTacToeRuleEngine();
                ruleEngine.Setup(new TicTacToeConfiguration(
                    requiredNumberOfPlayers: dto.NumberOfPlayers,
                    numberOfRounds: dto.NumberOfRounds,
                    requiredNumberOfWins: dto.RequiredNumberOfWins,
                    description: dto.EndOfGameMessage));
                ruleEngine.Initialize();
                return ruleEngine; 
            }
            default:
            {
                throw new NotImplementedException($"Game {dto.Title} doesn't exist.");
            }
        }
    }
}