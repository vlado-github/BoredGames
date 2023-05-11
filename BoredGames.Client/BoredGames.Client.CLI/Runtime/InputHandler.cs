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
            Console.Write(">>> Choose game title (e.g. 0,1,2...):");
            var gameTitle = Console.ReadLine();
            var gameTitleId = string.IsNullOrEmpty(gameTitle) ? 0 : int.Parse(gameTitle);
            Console.Write(">>> Number of players:");
            var numOfPlayers = Console.ReadLine();
            Console.Write(">>> Number of wins:");
            var numOfWins = Console.ReadLine();
            Console.Write(">>> Bet description:");
            var description = Console.ReadLine();
            result.GameDefinition = await _boredGamesApi.CreateGame(new CreateGameRequest
            {
                GameTitle = gameTitleId,
                NumberOfPlayers = Int32.Parse(numOfPlayers),
                NumberOfWins = Int32.Parse(numOfWins),
                Description = description
            });
            result.Joined = true;
            result.GameState = await _boredGamesApi.GetGameState(result.GameDefinition.GameId.ToString());
            Console.WriteLine($"<<< GameID: {result.GameDefinition.GameId}");
        }
        else if (input == "join")
        {
            Console.Write(">>> Enter Game ID:");
            var gameIdInput = Console.ReadLine();
            if (string.IsNullOrEmpty(gameIdInput))
            {
                Console.WriteLine("You have to enter Game ID to join.");
            }
            var gameId = new Guid(gameIdInput);
            result.GameDefinition = await _boredGamesApi.Join(new JoinGameRequest()
            {
                GameId = gameId
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