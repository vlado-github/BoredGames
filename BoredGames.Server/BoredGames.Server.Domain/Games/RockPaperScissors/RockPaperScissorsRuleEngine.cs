using System.Reflection.Metadata;
using BoredGames.Common.Enums;
using BoredGames.Common.Exceptions;
using BoredGames.Server.Domain.Games.Base;
using BoredGames.Server.Domain.Games.Dtos;
using BoredGames.Server.Domain.Games.Entities;
using Microsoft.Extensions.Logging;

namespace BoredGames.Server.Domain.Games.RockPaperScissors;

public class RockPaperScissorsRuleEngine : SimultaneousGameRuleEngine<RockPaperScissorsConfiguration>
{
    public static readonly string RockAction = "rock";
    public static readonly string PaperAction = "paper";
    public static readonly string ScissorsAction = "scissors";

    public RockPaperScissorsRuleEngine(ILogger<SimultaneousGameRuleEngine<RockPaperScissorsConfiguration>> logger) 
        : base(logger)
    {
    }

    public override void Setup(RockPaperScissorsConfiguration? configuration)
    {
        _gameSetupBuilder
            .AddConfiguration(configuration ?? RockPaperScissorsConfiguration.Default)
            .AddResultResolver(ResolveResultAction);
    }

    public override RoundResult Handle(MoveDto dto)
    {
        if (_rounds.Current.GetMoves().Any(x => x.PlayerId == dto.PlayerId))
        {
            throw new InvalidActionException("Make move",
                $"Player with ID {dto.PlayerId} has already made a move for round {_rounds.Current.Number}.");
        }    
        return base.Handle(dto);
    }

    public override RoundResult GetCurrentRoundResult()
    {
        return new RoundResult(
            roundStatus: _rounds.Current.GetStatus(),
            roundNumber: _rounds.Current.Number);
    }

    private RoundResult ResolveResultAction(MoveDto moveDto)
    {
        var remainingCommands = new List<MoveDto>(_rounds.Current.GetMoves());
        remainingCommands.Remove(moveDto);
        var result = CheckRule(moveDto.ActionType, remainingCommands);
        var player = new Player(moveDto.PlayerId, moveDto.PlayerNickName);
        if (result == GameResult.Win)
        {
            _gameScore.AddWin(player, _rounds.Current.Number, moveDto.ActionType);
            
            var otherPlayerMove = remainingCommands.Last(x => x.PlayerId != moveDto.PlayerId);
            var otherPlayer = new Player(otherPlayerMove.PlayerId, otherPlayerMove.PlayerNickName);
            _gameScore.AddLoss(otherPlayer, _rounds.Current.Number, otherPlayerMove.ActionType);
        }
        else if (result == GameResult.Loss)
        {
            _gameScore.AddLoss(player, _rounds.Current.Number, moveDto.ActionType);
            
            var otherPlayerMove = remainingCommands.Last(x => x.PlayerId != moveDto.PlayerId);
            var otherPlayer = new Player(otherPlayerMove.PlayerId, otherPlayerMove.PlayerNickName);
            _gameScore.AddWin(otherPlayer, _rounds.Current.Number, otherPlayerMove.ActionType);
        }
        else
        {
            _gameScore.AddDraw(player, _rounds.Current.Number, moveDto.ActionType);
            
            var otherPlayerMove = remainingCommands.Last(x => x.PlayerId != moveDto.PlayerId);
            var otherPlayer = new Player(otherPlayerMove.PlayerId, otherPlayerMove.PlayerNickName);
            _gameScore.AddDraw(otherPlayer, _rounds.Current.Number, otherPlayerMove.ActionType);
        }
        
        _rounds.Current.Complete();

        if (_rounds.AreFinished() && !_gameScore.IsRequiredNumberOfWinsMet())
        {
            _rounds.AddExtraRound();
        }
        
        _rounds.Next();
        
        return new RoundResult(
            isPreviousRoundCompleted: true,
            roundStatus: _rounds.Current.GetStatus(),
            roundNumber: _rounds.Current.Number);
    }

    private GameResult CheckRule(string actionType, IList<MoveDto> remainingActions)
    {
        if (actionType == RockAction)
        {
            if (remainingActions.Any(m => m.ActionType == PaperAction))
            {
                return GameResult.Loss;
            }
            if (remainingActions.All(x => x.ActionType == RockAction))
            {
                return GameResult.Draw;
            }
            return GameResult.Win;
        }
        else if (actionType == PaperAction)
        {
            if (remainingActions.Any(m => m.ActionType == ScissorsAction))
            {
                return GameResult.Loss;
            }
            if (remainingActions.All(x => x.ActionType == PaperAction))
            {
                return GameResult.Draw;
            }
            return GameResult.Win;
        }
        else //Scissors
        {
            if (remainingActions.Any(m => m.ActionType == RockAction))
            {
                return GameResult.Loss;
            }
            if (remainingActions.All(x => x.ActionType == ScissorsAction))
            {
                return GameResult.Draw;
            }
            return GameResult.Win;
        }
    }
}