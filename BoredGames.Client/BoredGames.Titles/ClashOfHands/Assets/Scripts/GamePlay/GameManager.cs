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
    [SerializeField] GameObject _playerHand;
    [SerializeField] GameObject _opponentHand;

    //[SerializeField] Texture _rockImage;
    //[SerializeField] Texture _paperImage;
    //[SerializeField] Texture _scissorsImage;

    //[SerializeField] RawImage selectedPlayerCardPlaceholder;
    //[SerializeField] RawImage selectedOpponentCardPlaceholder;

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
        //DisplayOpponentsSide(false);
       // DisplayPlayerSide(false);
        CheckGameStatus();
    }

    private void Update()
    {
        CheckGameStatus();
    }

    private void ShowNotification(string message, Color? textColor = null, float? duration = null)
    {
        _notificationManager.ShowNotification(message, textColor, duration);
    }

    //public void ShowSelectedCard(string cardTag, bool isOppenent)
    //{
    //    if (!string.IsNullOrEmpty(cardTag))
    //    {
    //        if (isOppenent)
    //        {
    //            GameState.Instance.CurrentRoundSelectedOpponentCard = cardTag;
    //            selectedOpponentCardPlaceholder.color = new Color(0, 0, 0, 1);
    //            if (cardTag == "rock")
    //            {
    //                selectedOpponentCardPlaceholder.texture = _rockImage;
    //            }
    //            else if (cardTag == "paper")
    //            {
    //                selectedOpponentCardPlaceholder.texture = _paperImage;
    //            }
    //            else
    //            {
    //                selectedOpponentCardPlaceholder.texture = _scissorsImage;
    //            }
    //        }
    //        else
    //        {
    //            GameState.Instance.CurrentRoundSelectedPlayerCard = cardTag;
    //            selectedPlayerCardPlaceholder.color = new Color(0, 0, 0, 1);
    //            if (cardTag == "rock")
    //            {
    //                selectedPlayerCardPlaceholder.texture = _rockImage;
    //            }
    //            else if (cardTag == "paper")
    //            {
    //                selectedPlayerCardPlaceholder.texture = _paperImage;
    //            }
    //            else
    //            {
    //                selectedPlayerCardPlaceholder.texture = _scissorsImage;
    //            }
    //        }
    //    }
    //}

    //private void RemoveSelectedCards()
    //{
    //    if (!string.IsNullOrEmpty(GameState.Instance.CurrentRoundSelectedPlayerCard))
    //    {
    //        selectedPlayerCardPlaceholder.texture = null;
    //        selectedPlayerCardPlaceholder.color = new Color(0,0,0,0);
    //        GameState.Instance.CurrentRoundSelectedPlayerCard = null;
    //    }
    //    if (!string.IsNullOrEmpty(GameState.Instance.CurrentRoundSelectedOpponentCard))
    //    {
    //        selectedOpponentCardPlaceholder.texture = null;
    //        selectedOpponentCardPlaceholder.color = new Color(0, 0, 0, 0);
    //        GameState.Instance.CurrentRoundSelectedOpponentCard = null;
    //    }
    //}

    //private void DisplayOpponentsSide(bool show = true)
    //{
    //    //Debug.Log($"DisplayOpponentsSide show:{show}");
    //    if (_opponentHand == null)
    //    {
    //        Debug.LogError($"_opponentHand can't be empty.");
    //        return;
    //    }
    //    _opponentHand.SetActive(show);
    //}

    //public void DisplayPlayerSide(bool show = true)
    //{
    //    //Debug.Log($"DisplayPlayerSide show:{show}");
    //    if (_playerHand == null)
    //    {
    //        _playerHand = GameObject.FindGameObjectWithTag("player_hand");
    //    }
    //    _playerHand.SetActive(show);
    //}

    private IEnumerator Delay(float seconds, Action callback = null)
    {
        yield return new WaitForSeconds(seconds);
        if (callback != null)
        {
            callback.Invoke();
        }
    }

    private void ResetHands()
    {
        if (string.IsNullOrEmpty(GameState.Instance.CurrentRoundSelectedOpponentCard) && string.IsNullOrEmpty(GameState.Instance.CurrentRoundSelectedPlayerCard))
        {
            if (_playerHand != null)
            {
                _playerHand.gameObject.SetActive(true);
            }
            if (_opponentHand != null)
            {
                _opponentHand.gameObject.SetActive(true);
            }
        }
    }

    private void HandleRoundResultDisplay()
    {
        var roundResult = GameState.Instance.Score.GetRoundResult(GameState.Instance.PreviousRoundNumber);
        Debug.Log($"prevRound: {GameState.Instance.PreviousRoundNumber}, {roundResult.Keys.FirstOrDefault()}:{roundResult.Values.FirstOrDefault()}, {roundResult.Keys.LastOrDefault()} {roundResult.Values.LastOrDefault()}");
        var playerRoundScore = roundResult.FirstOrDefault(x => x.Key == GameState.Instance.PlayerId);
        var opponentRoundScore = roundResult.FirstOrDefault(x => x.Key != GameState.Instance.PlayerId);
        if (opponentRoundScore.Value != null)
        {
            GameState.Instance.CurrentRoundSelectedOpponentCard = opponentRoundScore.Value.SelectedCardTag;
        }

        Debug.Log($" in play: opponent={GameState.Instance.CurrentRoundSelectedOpponentCard}  player={GameState.Instance.CurrentRoundSelectedPlayerCard} prevRoundCompl={GameState.Instance.IsPreviousRoundCompleted}");


        if (GameState.Instance.IsPreviousRoundCompleted
            && !string.IsNullOrEmpty(GameState.Instance.CurrentRoundSelectedOpponentCard)
            && !string.IsNullOrEmpty(GameState.Instance.CurrentRoundSelectedPlayerCard))
        {
            if (playerRoundScore.Value != null)
            {
                switch (playerRoundScore.Value.Result)
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
            }

            StartCoroutine(Delay(1, () =>
            {
                Debug.Log(">>> reset <<<");
                GameState.Instance.CompleteRoundResultDisplay();
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
                    if (GameState.Instance.CurrentRoundStatus == RoundStatus.InProgress && !string.IsNullOrEmpty(GameState.Instance.CurrentRoundSelectedPlayerCard))
                    {
                        return;
                    }

                    //Debug.Log($">>> gameplay {GameState.Instance.Status} <<<");
                   // RemoveSelectedCards();
                    _waitingForPlayerCanvas.gameObject.SetActive(true);
                    _scoreCanvas.gameObject.SetActive(false);
                    _playerHand.gameObject.SetActive(true);
                    _opponentHand.gameObject.SetActive(false);
                    
                    //DisplayOpponentsSide(false);
                    //DisplayPlayerSide(true);
                    
                    break;
                }
            case GameStatus.InPlay:
                {
                    if (!gameOnNotificationDisplayed)
                    {
                        ShowNotification("clash on!", Color.green, 0.5f);
                        gameOnNotificationDisplayed = true;
                    }

                   // Debug.Log($">>> gameplay {GameState.Instance.Status} <<<");

                    ResetHands();

                    _waitingForPlayerCanvas.gameObject.SetActive(false);
                    _scoreCanvas.gameObject.SetActive(true);
                    _playerNameCanvas.gameObject.SetActive(false);

                   // Debug.Log($"scores: {GameState.Instance.Score?.PlayerScores.Length}");

                    if (GameState.Instance.Score == null)
                    {
                        break;
                    }
                    if (GameState.Instance.IsRoundResultDisplayCompleted())
                    {
                        break;
                    }

                    HandleRoundResultDisplay();
                    break;
                }
            case GameStatus.Finished:
                {
                    Debug.Log($">>> gameplay {GameState.Instance.Status} <<<");

                    _waitingForPlayerCanvas.gameObject.SetActive(false);
                    _scoreCanvas.gameObject.SetActive(true);
                    _playerNameCanvas.gameObject.SetActive(false);

                    ShowNotification("game over", Color.white, 0.5f);
                    break;
                }
        }
    }

}
