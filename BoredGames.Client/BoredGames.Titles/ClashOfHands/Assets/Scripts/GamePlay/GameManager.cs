using Assets.Scripts;
using Assets.Scripts.BoredGames.API;
using Assets.Scripts.GamePlay;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] Canvas _waitingForPlayerCanvas;
    [SerializeField] Canvas _scoreCanvas;

    [SerializeField] string _playersCardsTag;
    [SerializeField] string _opponentsCardsTag;

    private void Awake()
    {
        CheckGameStatus();
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    private void Start()
    {
        if (GameState.Instance.IsGameCreated)
        {
            Debug.LogWarning(">> gameplay <<<");
            _waitingForPlayerCanvas.gameObject.SetActive(true);
            _scoreCanvas.gameObject.SetActive(false);

            ShowOpponentsSide(false);
            ShowPlayerSide(true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void ShowOpponentsSide(bool show = true)
    {
        GameObject[] objectsToHideAtStart = GameObject.FindGameObjectsWithTag(_opponentsCardsTag);
        foreach (GameObject objectToHide in objectsToHideAtStart)
        {
            objectToHide.SetActive(show);
        }
    }

    private void ShowPlayerSide(bool show = true)
    {
        GameObject[] objectsToShowAtStart = GameObject.FindGameObjectsWithTag(_playersCardsTag);
        foreach (GameObject objectToShow in objectsToShowAtStart)
        {
            objectToShow.SetActive(show);
        }
    }

    private void CheckGameStatus()
    {
        if (string.IsNullOrEmpty(GameState.Instance.GameId))
        {
            return;
        }
        
        StartCoroutine(BoredGamesClient.Instance.GetGameState((response) => {
            GameState.Instance.Status = (GameStatus)response.gameStatus;
            GameState.Instance.CurrentRoundNumber = response.roundNumber;
            GameState.Instance.CurrentRoundStatus = response.roundStatus;

            switch (GameState.Instance.Status)
            {
                case GameStatus.AwaitingPlayers:
                    {
                        _scoreCanvas.gameObject.SetActive(false);
                        ShowOpponentsSide(false);
                        break;
                    }
                case GameStatus.InPlay:
                    {
                        _scoreCanvas.gameObject.SetActive(true);
                        ShowOpponentsSide(true);
                        break;
                    }
            }
        }));
    }

}
