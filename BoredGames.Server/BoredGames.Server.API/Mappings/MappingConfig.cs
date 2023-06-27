using BoredGames.Server.API.ViewModels;
using BoredGames.Server.Domain.Games.Entities;
using Mapster;

namespace BoredGames.Server.API.Mappings;

public static class MappingConfig
{
    public static void RegisterMappings(this IServiceCollection services)
    {
        TypeAdapterConfig<Hand, RoundResultViewModel>
            .NewConfig()
            .Map(dest => dest.RoundNumber, src => src.RoundNumber)
            .Map(dest => dest.PlayerMove, src => src.ActionType);

        TypeAdapterConfig<PlayerStatistic, PlayerScoreViewModel>
            .NewConfig()
            .Map(dest => dest.PlayerId, src => src.PlayerId)
            .Map(dest => dest.RoundWins, src => src.RoundWins)
            .Map(dest => dest.RoundDraws, src => src.RoundDraws)
            .Map(dest => dest.RoundLosses, src => src.RoundLosses);

        TypeAdapterConfig<GameScore, GameScoreViewModel>
            .NewConfig()
            .Map(dest => dest.PlayerScores, src => src.PlayersStats)
            .Map(dest => dest.CurrentRound, src => src.CurrentRoundNumber)
            .Map(dest => dest.RequiredNumberOfWins, src => src.RequiredNumberOfWins);

        TypeAdapterConfig<GameDefinition, GameDefinitionViewModel>
            .NewConfig()
            .Map(dest => dest.GameId, src => src.GameId)
            .Map(dest => dest.Description, 
                src => src.Description)
            .Map(dest => dest.RequiredNumberOfPlayers, 
                src => src.RequiredNumberOfPlayers)
            .Map(dest => dest.RequiredNumberOfWins, 
                src => src.RequiredNumberOfWins);
    }
}