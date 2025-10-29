using FluentValidation;

namespace BoredGames.API.Models;

public class JoinGame
{
    public Guid GameId { get; set; }
}

public class JoinGameValidator : AbstractValidator<JoinGame>
{
    public JoinGameValidator()
    {
        RuleFor(x => x.GameId).NotEmpty().NotNull().NotEqual(Guid.Empty);
    }
}