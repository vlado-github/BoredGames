using Bogus;

namespace BoredGames.Server.Domain.Games.Entities;

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
    
    public Guid Id { get; private set; }
    public string NickName { get; private set; }
}