using System;
using BoredGames.Common.Enums;
using BoredGames.Server.Domain.Games.Dtos;
using BoredGames.Server.Domain.Games.Entities;

namespace BoredGames.Server.Domain.Games.Base;

public interface IGameSetupBuilder<T> where T : GameConfigurationBase
{
    IGameSetupBuilder<T> AddConfiguration(T gameConfiguration);
    IGameSetupBuilder<T> AddResultResolver(Func<MoveDto, RoundResult> resolver);
    IGameSetupBuilder<T> AddGameStateHandler(Action<ReadOnlyGameState> handler);
    IGameSetupBuilder<T> AddRoundCompletedHandler(Action handler);
    GameSetup<T> Build();
}