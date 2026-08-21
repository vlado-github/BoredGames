using System;
using System.Collections.Generic;
using BoredGames.Common.Enums;
using Orleans;

namespace BoredGames.Server.GameServer.ViewModels;

[GenerateSerializer]
public class GameStateViewModel
{
    [Id(0)]
    public Guid GameId { get; set; }
    [Id(1)]
    public GameStatus GameStatus { get; set; }
    [Id(2)]
    public int RoundNumber { get; set; }
    [Id(3)]
    public RoundStatus RoundStatus { get; set; }
    [Id(4)]
    public int PlayersNumber { get; set; }
    [Id(5)]
    public GameScoreViewModel Score { get; set; } = new ();
    [Id(6)] 
    public Guid? CurrentPlayerTurn { get; set; }
    [Id(7)]
    public Guid[]? PlayersTurnOrder { get; set; }
    [Id(8)]
    public IList<MoveViewModel> Moves { get; set; } = new List<MoveViewModel>();
    [Id(9)]
    public IList<PlayerViewModel> Players { get; set; } = new List<PlayerViewModel>();
    
}