using Assets.Scripts;
using Assets.Scripts.BoredGames.API;
using Assets.Scripts.GamePlay;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] TextMeshProUGUI _waitingMessage;
    [SerializeField] TextMeshProUGUI _score;
    [SerializeField] string _tagToHide;

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
        _waitingMessage.gameObject.SetActive(true);

        _score.gameObject.SetActive(false);
        GameObject[] objectsToHideAtStart = GameObject.FindGameObjectsWithTag(_tagToHide);
        foreach (GameObject objectToHide in objectsToHideAtStart)
        {
            objectToHide.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void CheckGameStatus()
    {
        StartCoroutine(BoredGamesClient.Instance.GetPlayerDetails((response) =>
        {
            GameState.Instance.PlayerId = response.id;
        }));

        StartCoroutine(BoredGamesClient.Instance.JoinGame((response) =>{}));
        
        StartCoroutine(BoredGamesClient.Instance.GetGameState((response) => {
            GameState.Instance.Status = (GameStatus)response.gameStatus;
            GameState.Instance.CurrentRoundNumber = response.roundNumber;
            GameState.Instance.CurrentRoundStatus = response.roundStatus;

            switch (GameState.Instance.Status)
            {
                case GameStatus.AwaitingPlayers:
                    {
                        _score.gameObject.SetActive(false);
                        GameObject[] objectsToHideAtStart = GameObject.FindGameObjectsWithTag(_tagToHide);
                        foreach (GameObject objectToHide in objectsToHideAtStart)
                        {
                            objectToHide.SetActive(false);
                        }
                        break;
                    }
                case GameStatus.InPlay:
                    {
                        _score.gameObject.SetActive(true);
                        GameObject[] objectsToHideAtStart = GameObject.FindGameObjectsWithTag(_tagToHide);
                        foreach (GameObject objectToHide in objectsToHideAtStart)
                        {
                            objectToHide.SetActive(true);
                        }
                        break;
                    }
            }
        }));
    }

}
