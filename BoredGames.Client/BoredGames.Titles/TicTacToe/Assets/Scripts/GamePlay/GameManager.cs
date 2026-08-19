using Assets.Scripts;
using Assets.Scripts.BoredGames.API;
using Assets.Scripts.GamePlay;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] Canvas _waitingForPlayerCanvas;
    [SerializeField] Canvas _scoreCanvas;
    [SerializeField] Canvas _playerNameCanvas;
    [SerializeField] Canvas _gameOverCanvas;
    [SerializeField] Canvas _tilesCanvas;
    [SerializeField] Canvas _playerCanvas;
    [SerializeField] Canvas _opponentCanvas;
    [SerializeField] GameObject _waitOpponentSpinner;
    [SerializeField] private GameObject _playerTurnIndicator;
    [SerializeField] private GameObject _opponentTurnIndicator;

    [SerializeField] NotificationFader _notificationManager;  

    bool gameOnNotificationDisplayed = false;
    bool gamePlayerTurnNotificationDisplayed = false;

    private void Awake()
    {
        if (Instance !=null && Instance != this)
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
                CheckGameStatus();
            }));
        }
    }

    public void CheckGameStatus()
    {
        Debug.Log($">>> CheckGameStatus {GameState.Instance.Status} <<<");
        Debug.Log($">>> Players {string.Join(",",GameState.Instance.Players.Select(x => x.NickName))} <<<");
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
                    _tilesCanvas.gameObject.SetActive(true);
                    _playerCanvas.gameObject.SetActive(true);
                    _opponentCanvas.gameObject.SetActive(false);
                    _waitOpponentSpinner.GameObject().SetActive(true);
                    
                    break;
                }
            case GameStatus.InPlay:
                {
                    Debug.Log($">>> current player turn {GameState.Instance.CurrentPlayerTurn} <<<");
                    Debug.Log($">>> player turns {string.Join(",",GameState.Instance.PlayersTurnOrder)} <<<");

                    if (!gameOnNotificationDisplayed)
                    {
                        ShowNotification("game on!", Color.green, 0.5f);
                        gameOnNotificationDisplayed = true;
                    }

                    _waitingForPlayerCanvas.gameObject.SetActive(false);
                    _scoreCanvas.gameObject.SetActive(true);
                    _playerNameCanvas.gameObject.SetActive(false);
                    _tilesCanvas.gameObject.SetActive(true);
                    _playerCanvas.gameObject.SetActive(true);
                    _waitOpponentSpinner.GameObject().SetActive(false);
                    _opponentCanvas.gameObject.SetActive(true);
                    
                    if (GameState.Instance.CurrentPlayerTurn == GameState.Instance.PlayerId)
                    {
                        _playerTurnIndicator.gameObject.SetActive(true);
                        _opponentTurnIndicator.gameObject.SetActive(false);
                        if (!gamePlayerTurnNotificationDisplayed)
                        {
                            ShowNotification("Your turn!", Color.green, 0.5f);
                            gamePlayerTurnNotificationDisplayed = true;
                        }
                    }
                    else
                    {
                        var player = GameState.Instance.Players.FirstOrDefault(x => x.Id == GameState.Instance.CurrentPlayerTurn);
                        if (player != null)
                        {
                            _opponentTurnIndicator.gameObject.SetActive(true);
                            _playerTurnIndicator.gameObject.SetActive(false);
                            if (!gamePlayerTurnNotificationDisplayed)
                            {
                                ShowNotification($"{player.NickName}'s turn", Color.orange, 0.5f);
                                gamePlayerTurnNotificationDisplayed = true;
                            }
                        }
                    }

                    break;
                }
            case GameStatus.Finished:
                {
                    //Debug.Log($">>> gameplay {GameState.Instance.Status} <<<");

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
