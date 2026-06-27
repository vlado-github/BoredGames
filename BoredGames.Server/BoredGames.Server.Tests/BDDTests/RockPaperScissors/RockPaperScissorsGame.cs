using BoredGames.API.Models;
using BoredGames.Common.Enums;
using BoredGames.Server.GameServer.Commands;
using BoredGames.Server.Tests.ApiClients;
using BoredGames.Server.Tests.Base;
using Refit;
using Xunit.Gherkin.Quick;

namespace BoredGames.Server.Tests.BDDTests.RockPaperScissors;

[FeatureFile("./BDDTests/RockPaperScissors/RockPaperScissorsGame.feature")]
public class RockPaperScissorsGame : BddDefinitionsBase
{
    private readonly Guid _player01 = new("fdd28238-aecb-4a4a-a7b0-a0ab43becf32");
    private readonly Guid _player02 = new("229b3b08-749c-48e9-8ca9-031914f83377");
    private readonly IGameClient _gameClient;
    private Guid _gameId;
  

    public RockPaperScissorsGame()
    {
        var baseAddress = AppHostInstance.CreateHttpClient("boredgames-client").BaseAddress?.ToString();
        _gameClient = RestService.For<IGameClient>(baseAddress!);
    }

    [Given("Game is created")]
    public async Task GivenTheGameIsCreated()
    {
        var command = new CreateGame()
        {
            GameTitle = GameTitle.ClashOfHands,
            NumberOfPlayers = 2,
            RequiredNumberOfConsecutiveWins = 1,
            NumberOfRounds = 1
        };
        var response = await _gameClient.Create(command);
        
        _gameId = response.GameId;
    }

    // [And("Second player joined")]
    // public async Task GivenTheSecondPlayerJoinedTheGame()
    // {
    //     var player02 = _grainFactory.GetGrain<IPlayerGrain>(_player02);
    //     await player02.JoinGame(new JoinGameCommand
    //     {
    //         GameId = _gameId
    //     });
    // }
    //
    // [And(@"Player ""(.+)"" made a move ""(.+)""")]
    // public async Task GivenPlayerMadeMove(string playerId, string actionType)
    // {
    //     var game = _grainFactory.GetGrain<IGameGrain>(_gameId);
    //     await game.MakeMove(new MakeMoveCommand()
    //     {
    //         ActionType = actionType,
    //         PlayerId = new Guid(playerId)
    //     });
    // }
    //
    // [When(@"Player ""(.+)"" makes a move ""(.+)""")]
    // public async Task WhenPlayerMakesMove(string playerId, string actionType)
    // {
    //     var game = _grainFactory.GetGrain<IGameGrain>(_gameId);
    //     await game.MakeMove(new MakeMoveCommand()
    //     {
    //         ActionType = actionType,
    //         PlayerId = new Guid(playerId)
    //     });
    // }
    //
    // [Then(@"Winner is player ""(.+)""")]
    // public async Task ThenWinnerShouldBePlayer(string playerId)
    // {
    //     var game = _grainFactory.GetGrain<IGameGrain>(_gameId);
    //     var result = await game.GetWinners();
    //     Assert.Single(result.Winners);
    //     Assert.Equal(result.Winners.Single().Id, new Guid(playerId));
    // }
    //
    // [Then("Game is a draw")]
    // public async Task ThenGameIsDraw()
    // {
    //     var game = _grainFactory.GetGrain<IGameGrain>(_gameId);
    //     var result = await game.GetWinners();
    //     Assert.Empty(result.Winners);
    // }
}
