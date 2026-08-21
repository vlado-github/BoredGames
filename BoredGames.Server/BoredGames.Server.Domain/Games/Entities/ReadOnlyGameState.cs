using System;
using System.Collections.Generic;
using BoredGames.Common.Enums;
using BoredGames.Server.Domain.Games.Dtos;

namespace BoredGames.Server.Domain.Games.Entities;

public class ReadOnlyGameState
{
    public ReadOnlyGameState(
        Guid gameId, 
        GameStatus gameStatus, 
        RoundStatus roundStatus, 
        int roundNumber, 
        IList<PlayerDto> players,
        Guid? currentPlayerTurn = null,
        Guid[]? playersTurnOrder = null)
    {
        GameId = gameId;
        GameStatus = gameStatus;
        RoundStatus = roundStatus;
        RoundNumber = roundNumber;
        Players = players;
        CurrentPlayerTurn = currentPlayerTurn;
        PlayersTurnOrder = playersTurnOrder;
    }
    
    public Guid GameId { get; private set; }
    public GameStatus GameStatus { get; private set; }
    public RoundStatus RoundStatus { get; private set; }
    public int RoundNumber { get; private set; }
    public Guid? CurrentPlayerTurn { get; private set; } = null;
    public Guid[]? PlayersTurnOrder {  get; private set; } = null;
    public int PlayersNumber => Players.Count;
    public IList<PlayerDto> Players { get; }
}