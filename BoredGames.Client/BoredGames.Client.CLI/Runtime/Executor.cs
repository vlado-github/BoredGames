using BoredGames.Client.CLI.API;
using BoredGames.Client.CLI.API.Base;
using BoredGames.Client.CLI.API.Requests;
using BoredGames.Client.CLI.API.Responses;

namespace BoredGames.Client.CLI.Runtime;

public class Executor : IExecutor
{
    private readonly IBoredGamesApi _boredGamesApi;
    private readonly ApiSettings _settings;
    
    public Executor(IBoredGamesApi boredGamesApi, ApiSettings settings)
    {
        _boredGamesApi = boredGamesApi;
        _settings = settings;
    }

    public async Task Execute()
    {
        try
        {
            Guid? gameId = null;
            var joined = false;
            var actionType = string.Empty;
            GameStateResponse? gameState = null;
            var waitingToJoinMessage = "Waiting for players to join...";
            var waitingForMoveMessage = "Waiting for other players to make a move...";

            Console.WriteLine("Welcome to BoredGames!");
            Console.WriteLine("-----------------------");
            Console.WriteLine($"Your PlayerID: {_settings.HeaderPlayerIdValue}");

            while (!joined)
            {
                Console.WriteLine("Type command (e.g. create, join, exit):");
                Console.Write(">");
                var input = Console.ReadLine();
                if (input == "create")
                {
                    gameId = await _boredGamesApi.CreateGame();
                    joined = true;
                    gameState = await _boredGamesApi.GetGameState(gameId.ToString());
                    Console.WriteLine($"*** Server ***: GameID: {gameId}");
                }
                else if (input == "join")
                {
                    Console.Write(">>> Enter Game ID:");
                    var gameIdInput = Console.ReadLine();
                    if (string.IsNullOrEmpty(gameIdInput))
                    {
                        Console.WriteLine("You have to enter Game ID to join.");
                    }
                    gameId = new Guid(gameIdInput);
                    gameState = await _boredGamesApi.Join(new JoinGameRequest()
                    {
                        GameId = gameId.Value
                    });
                    joined = true;
                }
                else if (input == "exit")
                {
                    return;
                }
                else
                {
                    Console.WriteLine($"Command '{input}' is not recognized.");
                }
            }

            if (gameState == null)
            {
                Console.WriteLine("Game state is null.");
                return;
            }

            var iteration = 0;
            while (gameState.State != GameStateEnum.Finished)
            {
                gameState = await _boredGamesApi.GetGameState(gameId.ToString());
                Thread.Sleep(2000);
                if (gameState.State == GameStateEnum.AwaitingPlayers)
                {
                    if (iteration == 0)
                    {
                        Console.Write(waitingToJoinMessage);
                        iteration++;
                    }
                    else
                    {
                        Console.Write(".");
                    }
                }
                else if (gameState.State == GameStateEnum.InPlay)
                {
                    if (!string.IsNullOrEmpty(actionType))
                    {
                        if (iteration == 1)
                        {
                            Console.Write(waitingForMoveMessage);
                        }
                        else
                        {
                            Console.Write(".");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Choose your action [R]ock/[P]aper/[S]cissors:");
                        var action = Console.ReadLine();
                        if (action.Equals("R", StringComparison.InvariantCultureIgnoreCase))
                        {
                            actionType = "rock";
                        }
                        else if (action.Equals("P", StringComparison.InvariantCultureIgnoreCase))
                        {
                            actionType = "paper";
                        }
                        else if (action.Equals("S", StringComparison.InvariantCultureIgnoreCase))
                        {
                            actionType = "scissors";
                        }
                        else
                        {
                            Console.WriteLine("Unrecognized action type.");
                        }

                        if (!string.IsNullOrEmpty(actionType))
                        {
                            gameState = await _boredGamesApi.MakeMove(new MakeMoveRequest(gameId.Value, actionType));
                        }
                    }
                }
            }

            var winners = await _boredGamesApi.GetGameWinners(gameId.ToString());
            if (winners.Contains(Guid.Parse(_settings.HeaderPlayerIdValue)))
            {
                Console.WriteLine("YOU WON!");
            }
            else
            {
                var endMessage = $"You lost! Winners: {string.Join(",", winners)}";
                Console.WriteLine(endMessage);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }
}