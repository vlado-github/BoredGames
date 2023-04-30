using BoredGames.Client.CLI.API;
using BoredGames.Client.CLI.API.Requests;

namespace BoredGames.Client.CLI.Runtime;

public class InputHandler
{
    private readonly IBoredGamesApi _boredGamesApi;

    public InputHandler(IBoredGamesApi boredGamesApi)
    {
        _boredGamesApi = boredGamesApi;
    }
    
    public async Task<ExecutionState> Handle()
    {
        var result = new ExecutionState();
        Console.WriteLine("Type command (e.g. create, join, exit):");
        Console.Write(">");
        var input = Console.ReadLine();
        if (input == "create")
        {
            Console.Write(">>> Number of players:");
            var numOfPlayers = Console.ReadLine();
            Console.Write(">>> Number of wins:");
            var numOfWins = Console.ReadLine();
            result.GameId = await _boredGamesApi.CreateGame(new CreateGameRequest
            {
                NumberOfPlayers = Int32.Parse(numOfPlayers),
                NumberOfWins = Int32.Parse(numOfWins)
            });
            result.Joined = true;
            result.GameState = await _boredGamesApi.GetGameState(result.GameId.ToString());
            Console.WriteLine($"<<< GameID: {result.GameId}");
        }
        else if (input == "join")
        {
            Console.Write(">>> Enter Game ID:");
            var gameIdInput = Console.ReadLine();
            if (string.IsNullOrEmpty(gameIdInput))
            {
                Console.WriteLine("You have to enter Game ID to join.");
            }
            result.GameId = new Guid(gameIdInput);
            result.GameState = await _boredGamesApi.Join(new JoinGameRequest()
            {
                GameId = result.GameId
            });
            result.Joined = true;
        }
        else if (input == "exit")
        {
            Environment.Exit(0);
        }
        else
        {
            Console.WriteLine($"Command '{input}' is not recognized.");
        }

        return result;
    }
}