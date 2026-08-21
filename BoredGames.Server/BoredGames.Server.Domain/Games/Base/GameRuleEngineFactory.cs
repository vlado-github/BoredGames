using System;
using BoredGames.Common.Enums;
using BoredGames.Server.Domain.Games.Dtos;
using BoredGames.Server.Domain.Games.RockPaperScissors;
using BoredGames.Server.Domain.Games.TicTacToe;
using Microsoft.Extensions.Logging;

namespace BoredGames.Server.Domain.Games.Base;

public static class GameRuleEngineFactory
{
    private static readonly ILoggerFactory LoggerFactory = new LoggerFactory();
    
    public static IGameRuleEngine GetInstance(GameDto dto, Action? onRoundCompleted = null)
    {
        switch (dto.Title)
        {
            case GameTitle.ClashOfHands:
            {
                var ruleEngine = new RockPaperScissorsRuleEngine(
                    new Logger<SimultaneousGameRuleEngine<RockPaperScissorsConfiguration>>(LoggerFactory));
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
                var ruleEngine = new TicTacToeRuleEngine(
                    new Logger<TurnBaseGameRuleEngine<TicTacToeConfiguration>>(LoggerFactory));
                ruleEngine.Setup(
                    configuration: new TicTacToeConfiguration(
                        requiredNumberOfPlayers: dto.NumberOfPlayers,
                        numberOfRounds: dto.NumberOfRounds,
                        requiredNumberOfWins: dto.RequiredNumberOfWins,
                        description: dto.EndOfGameMessage),
                    onRoundCompleted: onRoundCompleted);
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