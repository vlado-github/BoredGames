using BoredGames.Common.Enums;
using FluentValidation;

namespace BoredGames.API.Models;

public class CreateGame
{
    public GameTitle GameTitle { get; set; }
    public int NumberOfPlayers { get; set; } = 2;
    public int RequiredNumberOfConsecutiveWins { get; set; }
    public int NumberOfRounds { get; set; } = 100;
    public string? Description { get; set; }
}

public class CreateGameValidator : AbstractValidator<CreateGame>
{
    public CreateGameValidator()
    {
        RuleFor(x => x.GameTitle).NotNull();
        RuleFor(x => x.NumberOfPlayers).GreaterThan(0);
        RuleFor(x => x.RequiredNumberOfConsecutiveWins)
            .GreaterThan(0)
            .LessThanOrEqualTo(x => x.NumberOfRounds);
        RuleFor(x => x.NumberOfRounds)
            .GreaterThan(0)
            .GreaterThanOrEqualTo(x => x.RequiredNumberOfConsecutiveWins)
            .LessThan(100);
    }
}