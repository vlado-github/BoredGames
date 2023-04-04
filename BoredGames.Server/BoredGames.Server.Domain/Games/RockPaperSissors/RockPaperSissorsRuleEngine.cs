using BoredGames.Server.Common.Enums;
using BoredGames.Server.Domain.Commands;
using BoredGames.Server.Domain.Games.Base;

namespace BoredGames.Server.Domain.Games.RockPaperSissors;

public class RockPaperSissorsRuleEngine : IGameRuleEngine
{
    public static readonly string RockAction = "rock";
    public static readonly string PaperAction = "paper";
    public static readonly string SissorsAction = "sissors";
    
    public static readonly int MinimumRequiredNumberOfWins = 1;
    public static readonly int MinimumRequiredNumberOfPlayers = 2;
    
    private IList<MakeMoveCommand> _movesStack;
    private int _requiredNumberOfWins;
    private int _requiredNumberOfPlayers;
    private IDictionary<Guid, int> _gameStats;

    public RockPaperSissorsRuleEngine(int? numberOfWins, int? numberOfPlayers)
    {
        _movesStack = new List<MakeMoveCommand>();
        _gameStats = new Dictionary<Guid, int>();
        _requiredNumberOfWins = numberOfWins ?? MinimumRequiredNumberOfWins;
        _requiredNumberOfPlayers = numberOfPlayers ?? MinimumRequiredNumberOfPlayers;
    }

    public GameState Handle(MakeMoveCommand command)
    {
        _movesStack.Add(command);
        if (_movesStack.Count == _requiredNumberOfPlayers)
        {
            return ResolveResult();
        }

        return GameState.InPlay;
    }

    public IList<Guid> GetWinners()
    {
        return _gameStats.Where(gs => gs.Value == _requiredNumberOfWins)
            .Select(gs => gs.Key)
            .ToList();
    }

    private GameState ResolveResult()
    {
        foreach (var move in _movesStack)
        {
            var remainingCommands = new List<MakeMoveCommand>(_movesStack);
            remainingCommands.Remove(move);
            if (CheckRule(move.ActionType, remainingCommands))
            {
                int currentNumberOfWins = 0;
                _gameStats.TryGetValue(move.PlayerId, out currentNumberOfWins);
                _gameStats.Add(move.PlayerId, ++currentNumberOfWins);
            }
        }
        
        if (_gameStats.Any(gs => gs.Value == _requiredNumberOfPlayers))
        {
            return GameState.Finished;
        }
        return GameState.InPlay;
    }

    private bool CheckRule(string actionType, IList<MakeMoveCommand> remainingCommands)
    {
        if (actionType == RockAction)
        {
            if (remainingCommands.Any(m => m.ActionType == PaperAction))
            {
                return false;
            }
            return true;
        }
        else if (actionType == PaperAction)
        {
            if (remainingCommands.Any(m => m.ActionType == SissorsAction))
            {
                return false;
            }
            return true;
        }
        else //Sissors
        {
            if (remainingCommands.Any(m => m.ActionType == RockAction))
            {
                return false;
            }
            return true;
        }
    }
}