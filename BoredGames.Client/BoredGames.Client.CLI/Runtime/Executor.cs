using BoredGames.Client.CLI.API;
using BoredGames.Client.CLI.API.Base;
using BoredGames.Client.CLI.API.Responses;

namespace BoredGames.Client.CLI.Runtime;

public class Executor : IExecutor
{
    private readonly IBoredGamesApi _boredGamesApi;
    private readonly ApiSettings _settings;
    private readonly InputHandler _inputHandler;
    private readonly PlayHandler _playHandler;
    
    public Executor(IBoredGamesApi boredGamesApi, ApiSettings settings)
    {
        _boredGamesApi = boredGamesApi;
        _settings = settings;
        _inputHandler = new InputHandler(_boredGamesApi);
        _playHandler = new PlayHandler(_boredGamesApi);
    }

    public async Task Execute()
    {
        try
        {
            ExecutionState executionState = null;

            await ShowIntro();

            while (executionState == null || !executionState.Joined)
            {
                executionState = await _inputHandler.Handle();
            }

            while (executionState.State != GameStateEnum.Finished)
            {
                executionState = await _playHandler.Handle(executionState);
            }

            var winners = await _boredGamesApi.GetGameWinners(executionState.GameId.ToString());
            if (winners.Contains(Guid.Parse(_settings.HeaderPlayerIdValue)))
            {
                Console.WriteLine(string.Empty);
                Console.WriteLine("**********");
                Console.WriteLine("YOU WON!");
                Console.WriteLine("**********");
            }
            else
            {
                Console.WriteLine(string.Empty);
                Console.WriteLine("**********");
                var endMessage = $"You lost! Winner(s): {string.Join(",", winners)}";
                Console.WriteLine($"You need to {executionState.Description}");
                Console.WriteLine(endMessage);
                Console.WriteLine("**********");
            }
            
            var score = await _boredGamesApi.GetGameScore(executionState.GameId.ToString());
            ShowScore(executionState.GameScore);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }

    private async Task ShowIntro()
    {
        using (var reader = new StreamReader("asset-title.txt"))
        {
            var title = await reader.ReadToEndAsync();
            Console.WriteLine(title);
        }
        Console.WriteLine("-----------------------------------------------");
        Console.WriteLine($"Your PlayerID: {_settings.HeaderPlayerIdValue}");
    }

    private void ShowScore(GameScoreResponse? score)
    {
        if (score == null) return;
        
        Console.WriteLine($">>> Round {score.CurrentRound}/{score.RequiredNumberOfWins}");
        foreach (var playerScore in score.PlayerScores)
        {
            Console.WriteLine($">>> {playerScore.PlayerId} " +
                              $"Win/Loss: {playerScore.RoundWins.Count()} / {playerScore.RoundLosses.Count()}");
        }
    }
}