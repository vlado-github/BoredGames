using FluentValidation;

namespace BoredGames.Server.API.Models;

public class CreateGame
{
    public int NumberOfPlayers { get; set; }
    public int NumberOfWins { get; set; }
}

public class CreateGameValidator : AbstractValidator<CreateGame>
{
    public CreateGameValidator()
    {
        RuleFor(x => x.NumberOfPlayers).GreaterThan(0);
        RuleFor(x => x.NumberOfWins).GreaterThan(0);
    }
}