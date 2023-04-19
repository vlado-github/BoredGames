namespace BoredGames.Client.CLI.Runtime;

public class Executor : IExecutor
{
    public Executor()
    {
        
    }

    public void Execute()
    {
        Console.WriteLine("Welcome to BoredGames!");
        Console.ReadLine();
    }
}