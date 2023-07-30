using BoredGames.Server.Service.Commands;
using BoredGames.Server.Service.ViewModels;
using Orleans;

namespace BoredGames.Server.Service.Grains.Base;

public interface IGameGrain : IGrainWithGuidKey
{
    Task<GameDefinitionViewModel> AddPlayerToGame(AddPlayerCommand command);
    Task<GameStateViewModel> MakeMove(MakeMoveCommand command);
    Task<IList<PlayerViewModel>> GetWinners();
    Task<GameStateViewModel> GetState();
    Task<GameScoreViewModel> GetScore();
    void Setup(CreateGameCommand command);
}