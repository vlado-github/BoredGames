using Bogus;
using Orleans;

namespace BoredGames.Server.Domain.Games.Entities;

[GenerateSerializer]
public class Player
{
    public Player(Guid id)
    {
        Id = id;
        NickName = new Faker().Name.FirstName();
    }

    public Player(Guid id, string nickName)
    {
        Id = id;
        NickName = nickName;
    }
    
    [Id(0)]
    public Guid Id { get; private set; }
    [Id(1)]
    public string NickName { get; private set; }
}