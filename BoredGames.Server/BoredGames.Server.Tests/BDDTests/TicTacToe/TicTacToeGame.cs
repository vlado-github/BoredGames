using System;
using System.Linq;
using System.Threading.Tasks;
using BoredGames.Common.Enums;
using BoredGames.Server.GameServer.Commands;
using BoredGames.Server.GameServer.Grains.Base;
using BoredGames.Server.Tests.Base;
using Xunit.Gherkin.Quick;

namespace BoredGames.Server.Tests.BDDTests.TicTacToe;

[FeatureFile("./BDDTests/TicTacToe/TicTacToeGame.feature")]
public class TicTacToeGame : BddDefinitionsBase
{
    private readonly Guid _player01 = new("fdd28238-aecb-4a4a-a7b0-a0ab43becf32");
    private readonly Guid _player02 = new("229b3b08-749c-48e9-8ca9-031914f83377");
    private Guid _gameId;

    [Given("Game is created")]
    public async Task GivenTheGameIsCreated()
    {
        var playerGrain = Cluster.Client.GetGrain<IPlayerGrain>(_player01);
        var result = await playerGrain.CreateGame(new CreateGameCommand()
        {
            Title = GameTitle.TicTacToe,
            NumberOfPlayers = 2,
            NumberOfRounds = 1,
            RequiredNumberOfWins = 1
        });
        
        _gameId = result.GameId;
    }

    [And("Second player joined")]
    public async Task GivenTheSecondPlayerJoinedTheGame()
    {
        var player02 = Cluster.Client.GetGrain<IPlayerGrain>(_player02);
        await player02.JoinGame(new JoinGameCommand
        {
            GameId = _gameId
        });
    }
    
    [And(@"Player ""(.+)"" made a move ""(.+)"" at row (\d+) and column (\d+)")]
    public async Task GivenPlayerMadeMove(string playerId, string actionType, int row, int column)
    {
        var game = Cluster.Client.GetGrain<IGameGrain>(_gameId);
        await game.MakeMove(new MakeMoveCommand()
        {
            ActionType = actionType,
            PlayerId = new Guid(playerId),
            SelectedTileRow = row,
            SelectedTileColumn = column
        });
    }
    
    [When(@"Player ""(.+)"" makes a move ""(.+)"" at row (\d+) and column (\d+)")]
    public async Task WhenPlayerMakesMove(string playerId, string actionType, int row, int column)
    {
        var game = Cluster.Client.GetGrain<IGameGrain>(_gameId);
        await game.MakeMove(new MakeMoveCommand()
        {
            ActionType = actionType,
            PlayerId = new Guid(playerId),
            SelectedTileRow = row,
            SelectedTileColumn = column
        });
    }
    
    [Then(@"Winner is player ""(.+)""")]
    public async Task ThenWinnerShouldBePlayer(string playerId)
    {
        var game = Cluster.Client.GetGrain<IGameGrain>(_gameId);
        var result = await game.GetWinners();
        Assert.Single(result.Winners);
        Assert.Equal(result.Winners.Single().Id, new Guid(playerId));
    }
    
    [Then("Game is a draw")]
    public async Task ThenGameIsDraw()
    {
        var game = Cluster.Client.GetGrain<IGameGrain>(_gameId);
        var result = await game.GetWinners();
        Assert.Empty(result.Winners);
    }
}
