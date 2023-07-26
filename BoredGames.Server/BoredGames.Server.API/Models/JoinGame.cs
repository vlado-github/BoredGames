using BoredGames.Server.Common.Enums;
using FluentValidation;

namespace BoredGames.Server.API.Models;

public class JoinGame
{
    public Guid GameId { get; set; }
    public string? PlayerNickName { get; set; }
}

public class JoinGameValidator : AbstractValidator<JoinGame>
{
    public JoinGameValidator()
    {
        RuleFor(x => x.GameId).NotEmpty().NotNull().NotEqual(Guid.Empty);
    }
}