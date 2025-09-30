using BoredGames.Common.Enums;
using BoredGames.Server.Domain.Games.RockPaperScissors;

namespace BoredGames.Server.Domain.Games.Base;

public static class GameFormSchemaFactory
{
    public static IFormSchema GetInstance(GameTitle title)
    {
        switch (title)
        {
            case GameTitle.RockPaperScissors:
            {
                return new RockPaperScissorsFormSchema();
            }
            default:
            {
                throw new NotImplementedException($"Game {title} doesn't exist.");
            }
        }
    }
}