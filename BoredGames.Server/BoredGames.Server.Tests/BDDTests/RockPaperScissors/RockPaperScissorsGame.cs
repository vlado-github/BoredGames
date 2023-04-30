using BoredGames.Server.Domain.Commands;
using BoredGames.Server.Domain.Grains.Base;
using BoredGames.Server.Tests.Base;
using Microsoft.Extensions.DependencyInjection;
using Orleans;
using Xunit.Gherkin.Quick;

namespace BoredGames.Server.Tests.BDDTests.RockPaperScissors;

[FeatureFile("./BDDTests/RockPaperScissors/RockPaperScissorsGame.feature")]
public class RockPaperScissorsGame : BddDefinitionsBase
{
    private readonly IGrainFactory _grainFactory;
    private readonly Guid _player01;
    private readonly Guid _player02;
    private Guid _gameId;

    public RockPaperScissorsGame() : base()
    {
        using (var scope = Application.Services.CreateScope())
        {
            _grainFactory = scope.ServiceProvider.GetRequiredService<IGrainFactory>();
        }
        _player01 = new Guid("fdd28238-aecb-4a4a-a7b0-a0ab43becf32");
        _player02 = new Guid("229b3b08-749c-48e9-8ca9-031914f83377");
    } 
    
    [Given("Game is created")]
    public async Task GivenTheGameIsCreated()
    {
        var player01 = _grainFactory.GetGrain<IPlayerGrain>(_player01);
        var command = new CreateGameCommand
        {
            NumberOfPlayers = 2,
            NumberOfWins = 1,
        };
        _gameId = await player01.CreateGame(command);
    }

    [And("Second player joined")]
    public async Task GivenTheSecondPlayerJoinedTheGame()
    {
        var player02 = _grainFactory.GetGrain<IPlayerGrain>(_player02);
        await player02.JoinGame(_gameId);
    }
    
    [And(@"Player ""(.+)"" made a move ""(.+)""")]
    public async Task GivenPlayerMadeMove(string playerId, string actionType)
    {
        var game = _grainFactory.GetGrain<IGameGrain>(_gameId);
        await game.MakeMove(new MakeMoveCommand()
        {
            ActionType = actionType,
            PlayerId = new Guid(playerId)
        });
    }

    [When(@"Player ""(.+)"" makes a move ""(.+)""")]
    public async Task WhenPlayerMakesMove(string playerId, string actionType)
    {
        var game = _grainFactory.GetGrain<IGameGrain>(_gameId);
        await game.MakeMove(new MakeMoveCommand()
        {
            ActionType = actionType,
            PlayerId = new Guid(playerId)
        });
    }

    [Then(@"Winner is player ""(.+)""")]
    public async Task ThenWinnerShouldBePlayer(string playerId)
    {
        var game = _grainFactory.GetGrain<IGameGrain>(_gameId);
        var winners = await game.GetWinners();
        Assert.Single(winners);
        Assert.Equal(winners.Single(), new Guid(playerId));
    }
    
    [Then("Game is a draw")]
    public async Task ThenGameIsDraw()
    {
        var game = _grainFactory.GetGrain<IGameGrain>(_gameId);
        var winners = await game.GetWinners();
        Assert.Empty(winners);
    }
}