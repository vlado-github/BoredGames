using Bogus;
using Bogus.DataSets;
using BoredGames.Server.Domain.Games.Base;
using BoredGames.Server.Domain.Games.Dtos;
using BoredGames.Server.Domain.Games.Entities;
using BoredGames.Server.Service.Commands;
using BoredGames.Server.Service.ViewModels;
using Mapster;
using Microsoft.Extensions.DependencyInjection;

namespace BoredGames.Server.Service.Mappings;

public static class MappingConfig
{
    public static void RegisterMappings(this IServiceCollection services)
    {
        var faker = new Faker();
        
        TypeAdapterConfig<CreateGameCommand, GameDto>
            .NewConfig();
        
        TypeAdapterConfig<MakeMoveCommand, MoveDto>
            .NewConfig();
        
        TypeAdapterConfig<AddPlayerCommand, PlayerDto>
            .NewConfig();
        
        TypeAdapterConfig<GameState, GameStateViewModel>
            .NewConfig();
        
        TypeAdapterConfig<GameConfigurationBase, GameDefinitionViewModel>
            .NewConfig();
        
        TypeAdapterConfig<Hand, RoundResultViewModel>
            .NewConfig()
            .Map(dest => dest.RoundNumber, src => src.RoundNumber)
            .Map(dest => dest.PlayerMove, src => src.ActionType);

        TypeAdapterConfig<PlayerStatistic, PlayerScoreViewModel>
            .NewConfig()
            .Map(dest => dest.PlayerId, src => src.PlayerId)
            .Map(dest => dest.PlayerNickName, src => src.PlayerNickName)
            .Map(dest => dest.RoundWins, src => src.RoundWins)
            .Map(dest => dest.RoundDraws, src => src.RoundDraws)
            .Map(dest => dest.RoundLosses, src => src.RoundLosses);

        TypeAdapterConfig<GameScore, GameScoreViewModel>
            .NewConfig()
            .Map(dest => dest.RequiredNumberOfWins, src => src.RequiredNumberOfWins)
            .Map(dest => dest.PlayerScores, src => src.PlayerStatistics)
            .Map(dest => dest.CurrentRound, src => src.CurrentRoundNumber);

        TypeAdapterConfig<GameDefinition, GameDefinitionViewModel>
            .NewConfig()
            .Map(dest => dest.GameId, src => src.GameId)
            .Map(dest => dest.Description, 
                src => src.Description)
            .Map(dest => dest.RequiredNumberOfPlayers, 
                src => src.RequiredNumberOfPlayers)
            .Map(dest => dest.RequiredNumberOfWins, 
                src => src.RequiredNumberOfConsecutiveWins);
    }
}