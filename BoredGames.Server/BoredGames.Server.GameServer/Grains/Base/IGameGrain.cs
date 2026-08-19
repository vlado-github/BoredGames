using BoredGames.Server.GameServer.Commands;
using BoredGames.Server.GameServer.ViewModels;

namespace BoredGames.Server.GameServer.Grains.Base;

public interface IGameGrain : IGrainWithGuidKey
{
    Task AddPlayerToGame(AddPlayerCommand command);
    Task<GameStateViewModel> MakeMove(MakeMoveCommand command);
    Task<GameWinnersViewModel> GetWinners();
    Task<GameStateViewModel> GetState();
    Task<GameScoreViewModel> GetScore();
    Task<GameDefinitionViewModel> GetDefinition();
    Task Setup(CreateGameCommand command);
}