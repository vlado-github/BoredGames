using BoredGames.Server.Common.Enums;
using BoredGames.Server.Domain.Commands;
using BoredGames.Server.Domain.Games.Base;
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

    public override RoundResult Handle(MakeMoveCommand command)
    {
        _rounds.Current.AddMove(command);
        if (_rounds.Current.GetMoves().Count == _settings.RequiredNumberOfPlayers)
        {
            return ResolveResult();
        }

        return new RoundResult(
            roundStatus: _rounds.Current.GetStatus(), 
            roundNumber: _rounds.Current.Number);
    }
    
    public override GameScore GetScore()
    {
        return _gameScore;
    }

    public override IList<Guid> GetWinners()
    {
        return _gameScore.GetWinners();
    }

    public override RockPaperScissorsConfiguration GetConfiguration()
    {
        return _settings;
    }

    public override bool AreAllRoundsFinished()
    {
        return _rounds.AreFinished();
    }

    public override RoundResult GetCurrentRoundResult()
    {
        return new RoundResult(
            roundStatus: _rounds.Current.GetStatus(),
            roundNumber: _rounds.Current.Number);
    }

    private RoundResult ResolveResult()
    {
        foreach (var move in _rounds.Current.GetMoves())
        {
            var remainingCommands = new List<MakeMoveCommand>(_rounds.Current.GetMoves());
            remainingCommands.Remove(move);
            var result = CheckRule(move.ActionType, remainingCommands);
            if (result == GameResult.Win)
            {
                _gameScore.AddWin(move.PlayerId, _rounds.Current.Number, move.ActionType);
            }
            else if (result == GameResult.Loss)
            {
                _gameScore.AddLoss(move.PlayerId, _rounds.Current.Number, move.ActionType);
            }
            else
            {
                _gameScore.AddDraw(move.PlayerId, _rounds.Current.Number, move.ActionType);
            }
        }
        
        _rounds.Next();
        
        return new RoundResult(
            roundStatus: _rounds.Current.GetStatus(),
            roundNumber: _rounds.Current.Number);
    }

    private GameResult CheckRule(string actionType, IList<MakeMoveCommand> remainingCommands)
    {
        if (actionType == RockAction)
        {
            if (remainingCommands.Any(m => m.ActionType == PaperAction))
            {
                return GameResult.Loss;
            }
            if (remainingCommands.All(x => x.ActionType == RockAction))
            {
                return GameResult.Draw;
            }
            return GameResult.Win;
        }
        else if (actionType == PaperAction)
        {
            if (remainingCommands.Any(m => m.ActionType == ScissorsAction))
            {
                return GameResult.Loss;
            }
            if (remainingCommands.All(x => x.ActionType == PaperAction))
            {
                return GameResult.Draw;
            }
            return GameResult.Win;
        }
        else //Scissors
        {
            if (remainingCommands.Any(m => m.ActionType == RockAction))
            {
                return GameResult.Loss;
            }
            if (remainingCommands.All(x => x.ActionType == ScissorsAction))
            {
                return GameResult.Draw;
            }
            return GameResult.Win;
        }
    }
}