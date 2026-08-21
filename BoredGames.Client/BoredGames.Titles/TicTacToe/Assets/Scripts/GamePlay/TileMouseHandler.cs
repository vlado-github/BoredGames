using Assets.Scripts.BoredGames.API;
using Assets.Scripts.GamePlay;
using System;
using System.Linq;
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
 
    private string value;

    void Start()
    {
        button.onClick.AddListener(OnMouseDown);
    }

    void OnMouseDown()
    {
        if (!GameState.Instance.IsPlayerTurn() 
            || GameState.Instance.Status != Assets.Scripts.GameStatus.InPlay
            || GameState.Instance.Moves.Any(x => x.SelectedTile.Column == _column && x.SelectedTile.Row == _row))
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
                SelectedTile = new TilePosition
                {
                    Row = _row,
                    Column = _column
                }
            });
            value = actionType;
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
    }
}
