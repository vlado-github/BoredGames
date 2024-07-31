using BoredGames.Server.Common.Enums;
using FluentValidation;

namespace BoredGames.Server.API.Models;

public class CreateGame
{
    public GameTitle GameTitle { get; set; }
    public int NumberOfPlayers { get; set; }
    public int RequiredNumberOfWins { get; set; }
    public int NumberOfRounds { get; set; }
    public string? Description { get; set; }
    public string? PlayerNickName { get; set; }
}

public class CreateGameValidator : AbstractValidator<CreateGame>
{
    public CreateGameValidator()
    {
        RuleFor(x => x.GameTitle).NotNull();
        RuleFor(x => x.NumberOfPlayers).GreaterThan(0);
        RuleFor(x => x.RequiredNumberOfWins)
            .GreaterThan(0)
            .LessThanOrEqualTo(x => x.NumberOfRounds);
        RuleFor(x => x.NumberOfRounds)
            .GreaterThan(0)
            .GreaterThanOrEqualTo(x => x.RequiredNumberOfWins)
            .LessThan(100);
    }
}