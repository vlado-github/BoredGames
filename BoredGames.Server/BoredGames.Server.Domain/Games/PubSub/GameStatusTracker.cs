using BoredGames.Server.Domain.Games.Entities;

namespace BoredGames.Server.Domain.Games.PubSub;

public class GameStateTracker : IObservable<GameState>
{
    public GameStateTracker()
    {
        observers = new List<IObserver<GameState>>();
    }

    private List<IObserver<GameState>> observers;

    public IDisposable Subscribe(IObserver<GameState> observer)
    {
        if (!observers.Contains(observer))
        {
            observers.Add(observer);
        }

        return new Unsubscriber(observers, observer);
    }

    private class Unsubscriber : IDisposable
    {
        private List<IObserver<GameState>> _observers;
        private IObserver<GameState> _observer;

        public Unsubscriber(List<IObserver<GameState>> observers, IObserver<GameState> observer)
        {
            _observers = observers;
            _observer = observer;
        }

        public void Dispose()
        {
            if (_observers.Contains(_observer))
            {
                _observers.Remove(_observer);
            }
        }
    }

    public void TrackGameState(GameState gameState)
    {
        foreach (var observer in observers)
        {
            if (gameState == null)
            {
                observer.OnError(new Exception("Game state is null"));
            }
            else
            {
                observer.OnNext(gameState);
            }
        }
    }

    public void EndTransmission()
    {
        foreach (var observer in observers.ToArray())
        {
            if (observers.Contains(observer))
            {
                observer.OnCompleted();
            }
        }

        observers.Clear();
    }
}