using FluentValidation;

namespace BoredGames.API.Models;

public class JoinGame
{
    public Guid GameId { get; set; }
    public string PlayerNickName { get; set; } = string.Empty;
}

public class JoinGameValidator : AbstractValidator<JoinGame>
{
    public JoinGameValidator()
    {
        RuleFor(x => x.GameId).NotEmpty().NotNull().NotEqual(Guid.Empty);
        RuleFor(x => x.PlayerNickName).NotEmpty().NotNull();
    }
}