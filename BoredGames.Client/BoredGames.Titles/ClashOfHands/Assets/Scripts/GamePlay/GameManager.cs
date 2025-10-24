using Assets.Scripts;
using Assets.Scripts.BoredGames.API;
using Assets.Scripts.GamePlay;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] Canvas _waitingForPlayerCanvas;
    [SerializeField] Canvas _scoreCanvas;
    [SerializeField] Canvas _playerNameCanvas;

    [SerializeField] string _playersCardsTags;
    [SerializeField] string _opponentsCardsTag;

    IList<GameObject> objectsToHideAtStart;
    IList<GameObject> objectsToShowAtStart;

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
        objectsToHideAtStart = GameObject.FindGameObjectsWithTag(_opponentsCardsTag).ToList();

        var playersCardsTags = _playersCardsTags.Split(',');
        objectsToShowAtStart = new List<GameObject>();
        foreach (var playersCardsTag in playersCardsTags)
        {
            objectsToShowAtStart.Add(GameObject.FindGameObjectWithTag(playersCardsTag));
        }
        

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
        foreach (GameObject objectToHide in objectsToHideAtStart)
        {
            objectToHide.SetActive(show);
        }
    }

    private void ShowPlayerSide(bool show = true)
    {
        foreach (GameObject objectToShow in objectsToShowAtStart)
        {
            objectToShow.SetActive(show);
        }
    }

    public void CheckGameStatus()
    {
        if (!GameState.Instance.IsGameCreated || !GameState.Instance.IsPlayerSet)
        {
            return;
        }

        if (!BoredGamesSocketClient.Instance.IsConnected())
        {
            BoredGamesSocketClient.Instance.SetupConnection();
        }

        switch (GameState.Instance.Status)
        {
            case GameStatus.AwaitingPlayers:
                {
                    Debug.Log($">>> gameplay {GameState.Instance.Status} <<<");
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
                    _playerNameCanvas.gameObject.SetActive(false);
                    ShowOpponentsSide(true);
                    ShowPlayerSide(true);
                    break;
                }
            case GameStatus.Finished:
                {
                    Debug.Log($">>> gameplay {GameState.Instance.Status} <<<");

                    _waitingForPlayerCanvas.gameObject.SetActive(false);
                    _scoreCanvas.gameObject.SetActive(true);
                    _playerNameCanvas.gameObject.SetActive(false);
                    ShowOpponentsSide(true);
                    ShowPlayerSide(true);
                    break;
                }
        }
    }

}
