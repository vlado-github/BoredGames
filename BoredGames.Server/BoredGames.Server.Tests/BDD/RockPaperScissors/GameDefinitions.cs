using BoredGames.Server.Domain.Commands;
using BoredGames.Server.Domain.Grains.Base;
using BoredGames.Server.Tests.BDD.Base;
using Orleans;
using Xunit.Gherkin.Quick;

namespace BoredGames.Server.Tests.BDD.RockPaperScissors;

public class GameDefinitions : BDDDefinitionsBase
{
    private readonly Guid _player01;
    private readonly Guid _player02;
    private Guid _gameId;

    public GameDefinitions(IGrainFactory grainFactory) : base(grainFactory)
    {
        _player01 = Guid.NewGuid();
        _player02 = Guid.NewGuid();
    } 
    
    [Given("The game is created")]
    public async Task GivenTheGameIsCreated()
    {
        var player01 = _grainFactory.GetGrain<IPlayerGrain>(_player01);
        _gameId = await player01.CreateGame();
    }

    [Given("The second player joined")]
    public async Task GivenTheSecondPlayerJoinedTheGame()
    {
        var player02 = _grainFactory.GetGrain<IPlayerGrain>(_player02);
        await player02.JoinGame(_gameId);
    }

    [When("Player {} makes a move {}")]
    public async Task WhenPlayerMakesMove(Guid playerId, string actionType)
    {
        var game = _grainFactory.GetGrain<IGameGrain>(_gameId);
        await game.MakeMove(new MakeMoveCommand()
        {
            ActionType = actionType,
            PlayerId = _player01
        });
    }

    [Then("Winner is player {}")]
    public async Task ThenWinnerShouldBePlayer(Guid playerId)
    {
        var game = _grainFactory.GetGrain<IGameGrain>(_gameId);
        var winners = await game.GetWinners();
        Assert.Single(winners);
        Assert.Equal(winners.Single(), playerId);
    }
    
    [Then("Game is a draw")]
    public async Task ThenGameIsDraw()
    {
        var game = _grainFactory.GetGrain<IGameGrain>(_gameId);
        var winners = await game.GetWinners();
        Assert.Empty(winners);
    }
}