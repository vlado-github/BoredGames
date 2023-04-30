using BoredGames.Server.Common.Enums;
using BoredGames.Server.Domain.Commands;
using BoredGames.Server.Domain.Games.Base;
using BoredGames.Server.Domain.Games.Entities;

namespace BoredGames.Server.Domain.Games.RockPaperScissors;

public class RockPaperScissorsRuleEngine : IGameRuleEngine<RockPaperScissorsSettings>
{
    public static readonly string RockAction = "rock";
    public static readonly string PaperAction = "paper";
    public static readonly string ScissorsAction = "scissors";

    private Rounds _rounds;
    private RockPaperScissorsSettings _settings;
    private Statistic _stats;

    public RockPaperScissorsRuleEngine()
    {
        Setup(RockPaperScissorsSettings.Default);
    }

    public void Setup(RockPaperScissorsSettings settings)
    {
        _settings = settings;
        _rounds = new Rounds(_settings.RequiredNumberOfWins);
        _stats = new Statistic(_settings.RequiredNumberOfWins);
    }

    public GameState Handle(MakeMoveCommand command)
    {
        _rounds.Current.AddMove(command);
        if (_rounds.Current.GetMoves().Count == _settings.RequiredNumberOfPlayers)
        {
            return ResolveResult();
        }

        return GameState.InPlay;
    }

    public IList<Guid> GetWinners()
    {
        return _stats.GetWinners();
    }

    private GameState ResolveResult()
    {
        foreach (var move in _rounds.Current.GetMoves())
        {
            var remainingCommands = new List<MakeMoveCommand>(_rounds.Current.GetMoves());
            remainingCommands.Remove(move);
            if (CheckRule(move.ActionType, remainingCommands) == GameResult.Win)
            {
                _stats.AddWin(move.PlayerId);
            }
        }
        
        _rounds.Next();
        
        if (_rounds.AreFinished())
        {
            return GameState.Finished;
        }
        return GameState.InPlay;
    }

    private GameResult CheckRule(string actionType, IList<MakeMoveCommand> remainingCommands)
    {
        if (actionType == RockAction)
        {
            if (remainingCommands.Any(m => m.ActionType == PaperAction))
            {
                return GameResult.Lose;
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
                return GameResult.Lose;
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
                return GameResult.Lose;
            }
            if (remainingCommands.All(x => x.ActionType == ScissorsAction))
            {
                return GameResult.Draw;
            }
            return GameResult.Win;
        }
    }
}