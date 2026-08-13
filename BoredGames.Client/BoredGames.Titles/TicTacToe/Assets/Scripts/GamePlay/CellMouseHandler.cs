using Assets.Scripts.BoredGames.API;
using Assets.Scripts.GamePlay;
using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class CardMouseHandler : MonoBehaviour
{
    [SerializeField] int _row;
    [SerializeField] int _column;
    [SerializeField] private Button button;

    private string value = string.Empty;

    void Start()
    {
        button.onClick.AddListener(OnMouseDown);
    }

    void OnMouseDown()
    {
        Debug.Log(">>> OnMouseDown: " + _row.ToString() +", "+ _column.ToString());
        if (!GameState.Instance.IsPlayerTurn() 
            || GameState.Instance.Status != Assets.Scripts.GameStatus.InPlay
            || !string.IsNullOrEmpty(value))
        {
            return;
        }

        try
        {
            var actionType = GameState.Instance.GetActionType();
            BoredGamesSocketClient.Instance.MakeMove(new MakeMoveMessage
            {
                ActionType = actionType,
                GameId = GameState.Instance.GameId,
                PlayerId = GameState.Instance.PlayerId,
            });
            value = actionType;
        }
        catch (Exception ex)
        {
            Debug.LogException(ex);
            return;
        }
    }

    void OnMouseEnter()
    {
        Debug.Log(">>> OnMouseEnter");
        if (!GameState.Instance.IsPlayerTurn() 
            || GameState.Instance.Status != Assets.Scripts.GameStatus.InPlay
            || !string.IsNullOrEmpty(value))
        {
            return;
        }
    }

    void OnMouseExit()
    {
        Debug.Log(">>> OnMouseExit");
        if (!GameState.Instance.IsPlayerTurn() 
            || GameState.Instance.Status != Assets.Scripts.GameStatus.InPlay
            || !string.IsNullOrEmpty(value))
        {
            return;
        }
    }
}
