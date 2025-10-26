using Assets.Scripts;
using Assets.Scripts.BoredGames.API;
using Assets.Scripts.GamePlay;
using System;
using System.Collections;
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
    [SerializeField] GameObject _playerHand;
    [SerializeField] GameObject _opponentHand;

    [SerializeField] GameObject _rockPrefab;
    [SerializeField] GameObject _paperPrefab;
    [SerializeField] GameObject _scissorsPrefab;

    [SerializeField] NotificationFader _notificationManager;  

    GameObject opponentsSelectedCard = null;
    GameObject playersSelectedCard = null;
    bool gameOnNotificationDisplayed = false;

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
        DisplayOpponentsSide(false);
        DisplayPlayerSide(false);
        CheckGameStatus();
    }

    private void ShowNotification(string message, Color? textColor = null, float? duration = null)
    {
        _notificationManager.ShowNotification(message, textColor, duration);
    }

    public void ShowSelectedCard(string cardTag, bool isOppenent)
    {
        Debug.Log($"ShowSelectedCard {cardTag} {isOppenent}");
        GameObject selectedCard;
        if (!string.IsNullOrEmpty(cardTag))
        {
            if (cardTag == "rock")
            {
                selectedCard = Instantiate(_rockPrefab);
            } 
            else if (cardTag == "paper")
            {
                selectedCard = Instantiate(_paperPrefab);
            }
            else
            {
                selectedCard = Instantiate(_scissorsPrefab);
            }

            selectedCard.SetActive(true);

            if (isOppenent)
            {
                selectedCard.transform.position = new Vector3(0, 2.6f, 0);
                opponentsSelectedCard = selectedCard;
            }
            else
            {
                selectedCard.transform.position = new Vector3(0, -2.6f, 0);
                playersSelectedCard = selectedCard;
            }
        }
    }

    private void RemoveSelectedCards()
    {
        if (!GameState.Instance.CurrentRoundCardSelected && opponentsSelectedCard != null)
        {
            opponentsSelectedCard.SetActive(false);
        }
        if (!GameState.Instance.CurrentRoundCardSelected && playersSelectedCard != null)
        {
            playersSelectedCard.SetActive(false);
        }
    }

    private void DisplayOpponentsSide(bool show = true)
    {
        if (_opponentHand == null)
        {
            Debug.LogError($"_opponentHand can't be empty.");
            return;
        }
        Debug.Log($"_opponentHand {show}");
        var alpha = show ? 1f : 0f;
        _opponentHand.SetActive(show);
    }

    public void DisplayPlayerSide(bool show = true)
    {
        if (_playerHand == null)
        {
            Debug.LogError($"_playerHand can't be empty.");
            return;
        }
        Debug.Log($"_playerHand {show}");
        var alpha = show ? 1f : 0f;
        _playerHand.SetActive(show);
    }

    public void CheckRoundStatusAsync(int previousRoundNumber, bool previousRoundCompleted)
    {
        if (GameState.Instance.CurrentRoundStatus == RoundStatus.InProgress && previousRoundCompleted)
        {
            var score = GameState.Instance.Score;
            var playerScore = score.PlayerScores.FirstOrDefault(x => x.PlayerId == GameState.Instance.PlayerId);
            if (playerScore == null)
            {
                return;
            }
            var opponentScore = score.PlayerScores.FirstOrDefault(x => x.PlayerId != GameState.Instance.PlayerId);
            if (opponentScore == null) 
            {
                return;
            }
            

            DisplayOpponentsSide(false);
            var playersRoundScore = GetRoundResult(previousRoundNumber, playerScore);
            ShowSelectedCard(playersRoundScore.SelectedCardTag, isOppenent: false);

            var opponentsRoundScore = GetRoundResult(previousRoundNumber, opponentScore);
            ShowSelectedCard(opponentsRoundScore.SelectedCardTag, isOppenent: true);

            switch (playersRoundScore.Result)
            {
                case RoundScoreResultEnum.Win:
                    {
                        ShowNotification("Win", Color.green);
                        break;
                    }
                case RoundScoreResultEnum.Loss:
                    {
                        ShowNotification("Loss", Color.magenta);
                        break;
                    }
                case RoundScoreResultEnum.Draw:
                    {
                        ShowNotification("Draw", Color.yellow);
                        break;
                    }
                default:
                    {
                        throw new Exception("Round result is not supported.");
                    }
            }

            StartCoroutine(Delay(1, () =>
            {
                CheckGameStatus();
            }));
        }
    }

    private IEnumerator Delay(float seconds, Action callback)
    {
        yield return new WaitForSeconds(seconds);
        callback.Invoke();
    }

    private RoundScoreResult GetRoundResult(int previousRoundNumber, PlayerScore playerScore)
    {
        var result = new RoundScoreResult();
        var playerRoundWin = playerScore.RoundWins.FirstOrDefault(x => x.RoundNumber == previousRoundNumber);
        if (playerRoundWin == null)
        {
            var playerRoundLoss = playerScore.RoundLosses.FirstOrDefault(x => x.RoundNumber == previousRoundNumber);
            if (playerRoundLoss == null)
            {
                var playerRoundDraw = playerScore.RoundDraws.FirstOrDefault(x => x.RoundNumber == previousRoundNumber);
                if (playerRoundDraw != null)
                {
                    result.Result = RoundScoreResultEnum.Draw;
                    result.SelectedCardTag = playerRoundDraw.PlayerMove.ToLower();
                }
            }
            else
            {
                result.Result = RoundScoreResultEnum.Loss;
                result.SelectedCardTag = playerRoundLoss.PlayerMove.ToLower();
            }
        }
        else
        {
            result.Result = RoundScoreResultEnum.Win;
            result.SelectedCardTag = playerRoundWin.PlayerMove.ToLower();
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
                    RemoveSelectedCards();
                    _waitingForPlayerCanvas.gameObject.SetActive(true);
                    _scoreCanvas.gameObject.SetActive(false);
                    DisplayOpponentsSide(false);
                    DisplayPlayerSide(true);
                    
                    break;
                }
            case GameStatus.InPlay:
                {
                    if (GameState.Instance.CurrentRoundStatus == RoundStatus.InProgress && GameState.Instance.CurrentRoundCardSelected)
                    {
                        return;
                    }

                    if (!gameOnNotificationDisplayed)
                    {
                        ShowNotification("clash on!", Color.green, 0.5f);
                        gameOnNotificationDisplayed = true;
                    }

                    Debug.Log($">>> gameplay {GameState.Instance.Status} <<<");
                    RemoveSelectedCards();
                    _waitingForPlayerCanvas.gameObject.SetActive(false);
                    _scoreCanvas.gameObject.SetActive(true);
                    _playerNameCanvas.gameObject.SetActive(false);
                    DisplayOpponentsSide(true);
                    DisplayPlayerSide(true);
                   
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
