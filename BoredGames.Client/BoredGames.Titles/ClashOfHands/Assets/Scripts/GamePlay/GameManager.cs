using Assets.Scripts;
using Assets.Scripts.BoredGames.API;
using Assets.Scripts.GamePlay;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TMPro;
using Unity.VisualScripting;
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

    IDictionary<string, Vector3> objectsToShowAtStartDefaultPositions;
    IDictionary<string, Quaternion> objectsToShowAtStartDefaultRotations;

    GameObject opponentsSelectedCard = null;

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
        objectsToShowAtStartDefaultPositions = new Dictionary<string, Vector3>();
        objectsToShowAtStartDefaultRotations = new Dictionary<string, Quaternion>();

        var playersCardsTags = _playersCardsTags.Split(',');
        objectsToShowAtStart = new List<GameObject>();
        foreach (var playersCardsTag in playersCardsTags)
        {
            var playerCardObject = GameObject.FindGameObjectWithTag(playersCardsTag);
            objectsToShowAtStart.Add(playerCardObject);
            objectsToShowAtStartDefaultPositions.Add(playersCardsTag, playerCardObject.transform.position);
            objectsToShowAtStartDefaultRotations.Add(playersCardsTag, playerCardObject.transform.rotation);
        }
        

        ShowOpponentsSide(false);
        ShowPlayerSide(false);
        CheckGameStatus();
    }
    private void ShowNotification(string title, string message)
    {
        Debug.Log($" >>> {title.ToUpper()} <<<");
        Debug.Log($" @@@ {message.ToUpper()} @@@");
    }

    private void ShowOpponentsSelectedCard(string cardTag)
    {
        opponentsSelectedCard = Instantiate(GameObject.FindGameObjectWithTag(cardTag));
        opponentsSelectedCard.SetActive(true);
        opponentsSelectedCard.transform.position = new Vector3(0, 3.2f, 0);
    }

    private void RemoveOpponentsSelectedCard()
    {
        if (opponentsSelectedCard != null) 
        { 
            opponentsSelectedCard.SetActive(false); 
            opponentsSelectedCard = null; 
        }
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
            objectsToShowAtStartDefaultPositions.TryGetValue(objectToShow.tag, out var defaultPosition);
            objectsToShowAtStartDefaultRotations.TryGetValue(objectToShow.tag, out var defaultRotation);
            objectToShow.transform.position = defaultPosition;
            objectToShow.transform.rotation = defaultRotation;
        }
    }

    public void CheckRoundStatusAsync(int previousRoundNumber, bool previousRoundCompleted)
    {
        if (GameState.Instance.CurrentRoundStatus == RoundStatus.InProgress && previousRoundCompleted)
        {
            var score = GameState.Instance.Score;
            var opponentScore = score.PlayerScores.FirstOrDefault(x => x.PlayerId != GameState.Instance.PlayerId);
            if (opponentScore == null) 
            {
                return;
            }
            

            ShowOpponentsSide(false);
            var opponentsRoundScore = GetOpponentRoundResult(previousRoundNumber, opponentScore);
            Debug.Log("result:" + opponentsRoundScore.SelectedCardTag);
            ShowOpponentsSelectedCard(opponentsRoundScore.SelectedCardTag);
            //Task.Delay(2000);
            //RemoveOpponentsSelectedCard();


            //ShowOpponentsSide(true);
            //ShowPlayerSide(true);
        }
    }

    private RoundScoreResult GetOpponentRoundResult(int previousRoundNumber, PlayerScore opponentScore)
    {
        var result = new RoundScoreResult();
        var opponentRoundWin = opponentScore.RoundWins.FirstOrDefault(x => x.RoundNumber == previousRoundNumber);
        if (opponentRoundWin == null)
        {
            var opponentRoundLoss = opponentScore.RoundLosses.FirstOrDefault(x => x.RoundNumber == previousRoundNumber);
            if (opponentRoundLoss == null)
            {
                var opponentRoundDraw = opponentScore.RoundDraws.FirstOrDefault(x => x.RoundNumber == previousRoundNumber);
                if (opponentRoundDraw != null)
                {
                    result.Result = RoundScoreResultEnum.Draw;
                    result.SelectedCardTag = opponentRoundDraw.PlayerMove.ToLower();
                    ShowNotification($"Round {previousRoundNumber}", "Draw");
                }
            }
            else
            {
                result.Result = RoundScoreResultEnum.Loss;
                result.SelectedCardTag = opponentRoundLoss.PlayerMove.ToLower();
                ShowNotification($"Round {previousRoundNumber}", "Loss");
            }
        }
        else
        {
            result.Result = RoundScoreResultEnum.Win;
            result.SelectedCardTag = opponentRoundWin.PlayerMove.ToLower();
            ShowNotification($"Round {previousRoundNumber}", "Win");
        }
        

        return result;
    }


   

    public void CheckGameStatus()
    {
        Debug.Log($"CheckGameStatus {!GameState.Instance.IsGameCreated} {!GameState.Instance.IsPlayerSet}");
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
                    if (GameState.Instance.CurrentRoundStatus == RoundStatus.InProgress && GameState.Instance.CurrentRoundCardSelected)
                    {
                        return;
                    }

                    Debug.Log($">>> gameplay {GameState.Instance.Status} <<<");
                    _waitingForPlayerCanvas.gameObject.SetActive(true);
                    _scoreCanvas.gameObject.SetActive(false);
                    ShowOpponentsSide(false);
                    ShowPlayerSide(true);
                    break;
                }
            case GameStatus.InPlay:
                {
                    if (GameState.Instance.CurrentRoundStatus == RoundStatus.InProgress && GameState.Instance.CurrentRoundCardSelected)
                    {
                        return;
                    }

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
                    break;
                }
        }
    }

}
