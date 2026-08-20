using Assets.Scripts.BoredGames.API;
using Assets.Scripts.GamePlay;
using System;
using UnityEngine;
using UnityEngine.UI;

public class TileMouseHandler : MonoBehaviour
{
    [SerializeField] int _row;
    [SerializeField] int _column;
    [SerializeField] private Button button;
    [SerializeField] private Image image;
    [SerializeField] private Sprite exSprite;
    [SerializeField] private Sprite oxSprite;
 
    private string value = string.Empty;

    void Start()
    {
        button.onClick.AddListener(OnMouseDown);
    }

    void OnMouseDown()
    {
        if (!GameState.Instance.IsPlayerTurn() 
            || GameState.Instance.Status != Assets.Scripts.GameStatus.InPlay
            || !string.IsNullOrEmpty(value))
        {
            return;
        }

        try
        {
            var actionType = GameState.Instance.GetActionType();
            Debug.Log(">>> OnMouseDown: " + _row +", "+ _column + ", action:" + actionType);
            BoredGamesSocketClient.Instance.MakeMove(new MakeMoveMessage
            {
                ActionType = actionType,
                GameId = GameState.Instance.GameId,
                PlayerId = GameState.Instance.PlayerId,
                SelectedTile = new TilePosition
                {
                    Row = _row,
                    Column = _column
                }
            });
            value = actionType; 
            Debug.Log(">>> OnMouseDown: " + value);
            if (actionType.Equals("x", StringComparison.OrdinalIgnoreCase))
            {
                image.sprite = exSprite;
            }
            else
            {
                image.sprite = oxSprite;
            }
        }
        catch (Exception ex)
        {
            Debug.LogException(ex);
        }
        Debug.Log(">>> OnMouseDown: completed");
    }

    // void OnMouseEnter()
    // {
    //     Debug.Log(">>> OnMouseEnter");
    //     if (!GameState.Instance.IsPlayerTurn() 
    //         || GameState.Instance.Status != Assets.Scripts.GameStatus.InPlay
    //         || !string.IsNullOrEmpty(value))
    //     {
    //         return;
    //     }
    // }

    // void OnMouseExit()
    // {
    //     Debug.Log(">>> OnMouseExit");
    //     if (!GameState.Instance.IsPlayerTurn() 
    //         || GameState.Instance.Status != Assets.Scripts.GameStatus.InPlay
    //         || !string.IsNullOrEmpty(value))
    //     {
    //         return;
    //     }
    // }
}
