using BoredGames.Server.Domain.Games.Entities;
using Newtonsoft.Json;

namespace BoredGames.Server.Domain.Games.RockPaperScissors;

public class RockPaperScissorsFormSchema : GameFormSchema
{
    public RockPaperScissorsFormSchema()
    {
        Elements = new List<FormElement>()
        {
            new FormElement("formTitle")
            {
                Element = "h1",
                Children = "Create game",
            },
            
            new FormElement("requiredNumberOfConsecutiveWins")
            {
                Type = "range",
                Name = "requiredNumberOfConsecutiveWins",
                Value = "1",
                Step = "1",
                Validation =  "required|max:10",
                Min = "1",
                Max = "10",
                Label = "Required number of consecutive wins:",
            },
            new FormElement("requiredWinsValue")
            {
                Element = "pre",
                Children = "$get(requiredNumberOfConsecutiveWins).value",
                Attributes = new
                {
                    style = new Dictionary<string, string>()
                    {
                        { "font-size", "2em" }
                    }
                }
            }
        };
    }
}