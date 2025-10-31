using Assets.Scripts;
using Assets.Scripts.BoredGames.API;
using Assets.Scripts.GamePlay;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] Canvas _waitingForPlayerCanvas;
    [SerializeField] Canvas _scoreCanvas;
    [SerializeField] Canvas _playerNameCanvas;
    [SerializeField] Canvas _gameOverCanvas;
    [SerializeField] PlayerHandHandler _playerHand;
    [SerializeField] OpponentHandHandler _opponentHand;

    [SerializeField] SelectedPlayerCardHandler _selectedPlayerCardHandler;
    [SerializeField] SelectedOpponentCardHandler _selectedOpponentCardHandler;

    [SerializeField] NotificationFader _notificationManager;  

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
        CheckGameStatus();
    }

    private void ShowNotification(string message, Color? textColor = null, float? duration = null)
    {
        _notificationManager.ShowNotification(message, textColor, duration);
    }

    private IEnumerator Delay(float seconds, Action callback = null)
    {
        yield return new WaitForSeconds(seconds);
        if (callback != null)
        {
            callback.Invoke();
        }
    }

    public void HandleRoundResultDisplay()
    {
        var roundResult = GameState.Instance.Score.GetRoundResult(GameState.Instance.PreviousRoundNumber);
        var playerRoundScore = roundResult.FirstOrDefault(x => x.PlayerId == GameState.Instance.PlayerId);
        var opponentRoundScore = roundResult.FirstOrDefault(x => x.PlayerId != GameState.Instance.PlayerId);

        if (opponentRoundScore != null)
        {
            if (opponentRoundScore.RoundResult.Result == null)
            {
                return;
            }
            _opponentHand.Hide();
            _selectedOpponentCardHandler.Show(opponentRoundScore.RoundResult.SelectedCardTag);
        }

        if (playerRoundScore != null)
        {
            if (playerRoundScore.RoundResult.Result == null) 
            {
                return;
            }
            switch (playerRoundScore.RoundResult.Result)
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
                        ShowNotification("Tie", Color.yellow);
                        break;
                    }
                default:
                    {
                        throw new Exception("Round result is not supported.");
                    }
            }

            StartCoroutine(Delay(1, () =>
            {
                //Reset display for next round

                _selectedOpponentCardHandler.Hide();
                _selectedPlayerCardHandler.Hide();
                GameState.Instance.CurrentRoundSelectedPlayerCard = null;
                CheckGameStatus();
            }));
        }
    }

    public void CheckGameStatus()
    {
        if (!GameState.Instance.IsGameCreated || !GameState.Instance.IsPlayerSet)
        {
            return;
        }

        switch (GameState.Instance.Status)
        {
            case GameStatus.AwaitingPlayers:
                {
                    _waitingForPlayerCanvas.gameObject.SetActive(true);
                    _scoreCanvas.gameObject.SetActive(false);
                    
                    _playerHand.Show();
                    _opponentHand.Hide();
                    
                    break;
                }
            case GameStatus.InPlay:
                {
                    if (!gameOnNotificationDisplayed)
                    {
                        ShowNotification("clash on!", Color.green, 0.5f);
                        gameOnNotificationDisplayed = true;
                    }

                    _waitingForPlayerCanvas.gameObject.SetActive(false);
                    _scoreCanvas.gameObject.SetActive(true);
                    _playerNameCanvas.gameObject.SetActive(false);

                    if (string.IsNullOrEmpty(GameState.Instance.CurrentRoundSelectedPlayerCard))
                    {
                        _playerHand.Show();
                        _opponentHand.Show();
                    }

                    break;
                }
            case GameStatus.Finished:
                {
                    Debug.Log($">>> gameplay {GameState.Instance.Status} <<<");

                    _waitingForPlayerCanvas.gameObject.SetActive(false);
                    _scoreCanvas.gameObject.SetActive(true);
                    _playerNameCanvas.gameObject.SetActive(false);
                    _gameOverCanvas.gameObject.SetActive(true);

                    ShowNotification("game over", Color.white, 0.5f);

                    break;
                }
        }
    }

}
