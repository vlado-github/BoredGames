using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using BoredGames.Common.Enums;
using BoredGames.Server.Domain.Games.Base;
using BoredGames.Server.Domain.Games.Dtos;
using BoredGames.Server.Domain.Games.Entities;
using Microsoft.Extensions.Logging;

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

    public TicTacToeRuleEngine(ILogger<TurnBaseGameRuleEngine<TicTacToeConfiguration>> logger) : base(logger)
    {
    }

    public override void Setup(TicTacToeConfiguration? configuration, Action? onRoundCompleted = null)
    {
        _gameSetupBuilder
            .AddConfiguration(configuration ?? TicTacToeConfiguration.Default)
            .AddResultResolver(ResolveResultAction);
        if (onRoundCompleted != null)
        {
            _gameSetupBuilder.AddRoundCompletedHandler(onRoundCompleted);
        }
    }
    
    public override RoundResult GetCurrentRoundResult()
    {
        return new RoundResult(
            roundStatus: _rounds.Current.GetStatus(),
            roundNumber: _rounds.Current.Number,
            currentPlayerTurn: Turns?.Current.PlayerId,
            playersTurnOrder: Turns?.PlayersTurnOrder,
            moves: _rounds.Current.GetMoves());
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
                playersTurnOrder: Turns?.PlayersTurnOrder,
                moves: _rounds.Current.GetMoves()); 
        }
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

        CompleteRound();

        if (_rounds.AreFinished() && !_gameScore.IsRequiredNumberOfWinsMet())
        {
            _rounds.AddExtraRound();
        }
        
        _rounds.Next();
        
        return new RoundResult(
            isPreviousRoundCompleted: true,
            roundStatus: _rounds.Current.GetStatus(),
            roundNumber: _rounds.Current.Number,
            currentPlayerTurn: Turns?.Current.PlayerId,
            playersTurnOrder: Turns?.PlayersTurnOrder,
            moves: _rounds.Current.GetMoves());
    }

    private void CompleteRound()
    {
        _rounds.Current.Complete();
        Turns?.ReverseTurnOrder();
        _gameSetup.RoundCompletedHandlerAction?.Invoke();
    }

    /// <summary>
    /// Filters out all previous actions that match last move and compares them 
    /// against winning patterns.
    /// </summary>
    /// <param name="action"></param>
    /// <param name="remainingActions"></param>
    /// <returns></returns>
    private GameResult CheckRule(MoveDto action, IList<MoveDto> remainingActions)
    {
        if (remainingActions.Count == 0)
        {
            return GameResult.Pending;
        }
        
        var positions = remainingActions
            .Where(x => x.ActionType == action.ActionType)
            .Select(x => (row: x.SelectedTile.Row, column: x.SelectedTile.Column))
            .ToList();
        positions.Add((row: action.SelectedTile.Row, column: action.SelectedTile.Column));
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