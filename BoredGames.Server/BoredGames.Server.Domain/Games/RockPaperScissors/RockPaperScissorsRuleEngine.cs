using BoredGames.Common.Enums;
using BoredGames.Server.Domain.Games.Base;
using BoredGames.Server.Domain.Games.Dtos;
using BoredGames.Server.Domain.Games.Entities;

namespace BoredGames.Server.Domain.Games.RockPaperScissors;

public class RockPaperScissorsRuleEngine : GameRuleEngine<RockPaperScissorsConfiguration>
{
    //todo: refactor to move to SmartEnum
    public static readonly string RockAction = "rock";
    public static readonly string PaperAction = "paper";
    public static readonly string ScissorsAction = "scissors";

    public RockPaperScissorsRuleEngine()
    {
        Setup(RockPaperScissorsConfiguration.Default);
    }

    public override RoundResult Handle(MoveDto dto)
    {
        _rounds.Current.AddMove(dto);
        if (_rounds.Current.GetMoves().Count == _settings.RequiredNumberOfPlayers)
        {
            return ResolveResult();
        }

        return new RoundResult(
            roundStatus: _rounds.Current.GetStatus(), 
            roundNumber: _rounds.Current.Number);
    }

    public override RockPaperScissorsConfiguration GetDefinition()
    {
        return _settings;
    }
    
    private RoundResult ResolveResult()
    {
        foreach (var move in _rounds.Current.GetMoves())
        {
            var remainingCommands = new List<MoveDto>(_rounds.Current.GetMoves());
            remainingCommands.Remove(move);
            var result = CheckRule(move.ActionType, remainingCommands);
            var player = new Player(move.PlayerId, move.PlayerNickName);
            if (result == GameResult.Win)
            {
                _gameScore.AddWin(player, _rounds.Current.Number, move.ActionType);
            }
            else if (result == GameResult.Loss)
            {
                _gameScore.AddLoss(player, _rounds.Current.Number, move.ActionType);
            }
            else
            {
                _gameScore.AddDraw(player, _rounds.Current.Number, move.ActionType);
            }
        }
        
        _rounds.Current.Complete();

        if (_rounds.AreFinished() && !_gameScore.IsRequiredNumberOfWinsMet())
        {
            _rounds.AddExtraRound();
        }
        
        _rounds.Next();
        
        return new RoundResult(
            roundStatus: _rounds.Current.GetStatus(),
            roundNumber: _rounds.Current.Number);
    }

    private GameResult CheckRule(string actionType, IList<MoveDto> remainingDtos)
    {
        if (actionType == RockAction)
        {
            if (remainingDtos.Any(m => m.ActionType == PaperAction))
            {
                return GameResult.Loss;
            }
            if (remainingDtos.All(x => x.ActionType == RockAction))
            {
                return GameResult.Draw;
            }
            return GameResult.Win;
        }
        else if (actionType == PaperAction)
        {
            if (remainingDtos.Any(m => m.ActionType == ScissorsAction))
            {
                return GameResult.Loss;
            }
            if (remainingDtos.All(x => x.ActionType == PaperAction))
            {
                return GameResult.Draw;
            }
            return GameResult.Win;
        }
        else //Scissors
        {
            if (remainingDtos.Any(m => m.ActionType == RockAction))
            {
                return GameResult.Loss;
            }
            if (remainingDtos.All(x => x.ActionType == ScissorsAction))
            {
                return GameResult.Draw;
            }
            return GameResult.Win;
        }
    }
}