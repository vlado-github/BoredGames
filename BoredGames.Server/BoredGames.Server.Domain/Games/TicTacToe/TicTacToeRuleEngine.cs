using System.Numerics;
using BoredGames.Common.Enums;
using BoredGames.Server.Domain.Games.Base;
using BoredGames.Server.Domain.Games.Dtos;
using BoredGames.Server.Domain.Games.Entities;

namespace BoredGames.Server.Domain.Games.TicTacToe;

public class TicTacToeRuleEngine : TurnBaseGameRuleEngine<TicTacToeConfiguration>
{
    public static readonly string ExAction = "x";
    public static readonly string OxAction = "o";

    public static readonly List<List<(int, int)>> WinningPatterns = [
        [(0, 0), (0, 1), (0, 2)],
        [(1, 0), (1, 1), (1, 2)],
        [(2, 0), (2, 1), (2, 2)],
        [(0, 0), (1, 0), (2, 0)],
        [(0, 1), (1, 1), (2, 1)],
        [(0, 2), (1, 2), (2, 2)],
        [(0, 0), (1, 1), (2, 2)],
        [(0, 2), (1, 1), (2, 0)],
    ];
    
    public override void Setup(TicTacToeConfiguration? configuration)
    {
        _gameSetupBuilder
            .AddConfiguration(configuration ?? TicTacToeConfiguration.Default)
            .AddResultResolver(ResolveResultAction);
    }
    
    private RoundResult ResolveResultAction(MoveDto moveDto)
    {
        var remainingCommands = new List<MoveDto>(_rounds.Current.GetMoves());
        remainingCommands.Remove(moveDto);
        var result = CheckRule(moveDto, remainingCommands);
        if (result == GameResult.Pending)
        {
            return new RoundResult(
                roundStatus: _rounds.Current.GetStatus(),
                roundNumber: _rounds.Current.Number,
                currentPlayerTurn: Turns?.Current.PlayerId,
                nextPlayerTurn: Turns?.ShowNext(Turns.Current).PlayerId); 
        }
        var player = new Player(moveDto.PlayerId, moveDto.PlayerNickName);
        if (result == GameResult.Win)
        {
            _gameScore.AddWin(player, _rounds.Current.Number, moveDto.ActionType);
        }
        else if (result == GameResult.Loss)
        {
            _gameScore.AddLoss(player, _rounds.Current.Number, moveDto.ActionType);
        }
        else
        {
            _gameScore.AddDraw(player, _rounds.Current.Number, moveDto.ActionType);
        }
        
        _rounds.Current.Complete();

        if (_rounds.AreFinished() && !_gameScore.IsRequiredNumberOfWinsMet())
        {
            _rounds.AddExtraRound();
        }
        
        _rounds.Next();
        
        return new RoundResult(
            roundStatus: _rounds.Current.GetStatus(),
            roundNumber: _rounds.Current.Number,
            currentPlayerTurn: Turns?.Current.PlayerId,
            nextPlayerTurn: Turns?.ShowNext(Turns.Current).PlayerId);
    }

    private GameResult CheckRule(MoveDto action, IList<MoveDto> remainingActions)
    {
        if (remainingActions.Count == 0)
        {
            return GameResult.Pending;
        }
        
        var positions = remainingActions
            .Where(x => x.ActionType == action.ActionType)
            .Select(x => (row: x.Row, column: x.Column))
            .ToList();
        positions.Add((row: action.Row, column: action.Column));
        foreach (var winningPattern in WinningPatterns)
        {
            bool isSubset = !winningPattern.Except(positions).Any();
            if (isSubset)
            {
                return GameResult.Win;
            }
        }

        if (remainingActions.Count == 8)
        {
            return GameResult.Draw;
        }

        return GameResult.Pending;
    }
}