using System;
using System.Collections.Generic;
using BoredGames.Common.Consts;
using BoredGames.Common.Enums;
using BoredGames.Server.GameServer.ViewModels;

namespace BoredGames.API.Consts;

public static class GameTitles
{
    public static GameTitleViewModel ClashOfHands = new GameTitleViewModel()
    {
        Id = (int) GameTitle.ClashOfHands,
        Alias = nameof(GameTitle.ClashOfHands),
        Name = "Clash of Hands",
        Description = "A classic rock paper scissors game.",
        Tags = ["pvp","cards"],
        ThumbnailImageUrl = $"{Environment.GetEnvironmentVariable(EnvVarNames.AppBaseUrl)}/assets/{nameof(GameTitle.ClashOfHands).ToLower()}-logo.png",
    };
    
    public static GameTitleViewModel TicTacToe = new GameTitleViewModel()
    {
        Id = (int) GameTitle.TicTacToe,
        Alias = nameof(GameTitle.TicTacToe),
        Name = "Tic Tac Toe",
        Description = "A classic tic tac toe game.",
        Tags = ["pvp","board"],
        ThumbnailImageUrl = $"{Environment.GetEnvironmentVariable(EnvVarNames.AppBaseUrl)}/assets/{nameof(GameTitle.TicTacToe).ToLower()}-logo.png",
    };
    
    public static IList<GameTitleViewModel> All => new List<GameTitleViewModel>{ClashOfHands, TicTacToe};
}