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

    GameObject[] objectsToHideAtStart;
    GameObject[] objectsToShowAtStart;

    private void Awake()
    {
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
        objectsToHideAtStart = GameObject.FindGameObjectsWithTag(_opponentsCardsTag);
        objectsToShowAtStart = GameObject.FindGameObjectsWithTag(_playersCardsTag);

        ShowOpponentsSide(false);
        ShowPlayerSide(false);
        CheckGameStatus();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void ShowOpponentsSide(bool show = true)
    {
        Debug.Log($"{_opponentsCardsTag} {objectsToHideAtStart.Length}");
        foreach (GameObject objectToHide in objectsToHideAtStart)
        {
            objectToHide.SetActive(show);
        }
    }

    private void ShowPlayerSide(bool show = true)
    {
        Debug.Log($"{_playersCardsTag} {objectsToShowAtStart.Length}");
        foreach (GameObject objectToShow in objectsToShowAtStart)
        {
            objectToShow.SetActive(show);
        }
    }

    public void CheckGameStatus()
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
                        Debug.Log($">>> gameplay {GameState.Instance.Status} {_playersCardsTag} <<<");
                        _waitingForPlayerCanvas.gameObject.SetActive(true);
                        _scoreCanvas.gameObject.SetActive(false);
                        ShowOpponentsSide(false);
                        ShowPlayerSide(true);
                        break;
                    }
                case GameStatus.InPlay:
                    {
                        Debug.Log($">>> gameplay {GameState.Instance.Status} <<<");
                        _waitingForPlayerCanvas.gameObject.SetActive(false);
                        _scoreCanvas.gameObject.SetActive(true);
                        ShowOpponentsSide(true);
                        ShowPlayerSide(true);
                        break;
                    }
            }
        }));
    }

}
