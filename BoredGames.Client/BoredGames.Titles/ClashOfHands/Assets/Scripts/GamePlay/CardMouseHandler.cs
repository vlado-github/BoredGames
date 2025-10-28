using Assets.Scripts.BoredGames.API;
using Assets.Scripts.GamePlay;
using System;
using System.Linq;
using UnityEngine;

public class CardMouseHandler : MonoBehaviour
{
    [SerializeField] Sprite _card;
    [SerializeField] Sprite _highlightCard;
    [SerializeField] float popCardOffset;

    private Vector3 defaultPosition;

    private void Start()
    {
        defaultPosition = transform.position;
    }

    void OnMouseDown()
    {
        if (!string.IsNullOrEmpty(GameState.Instance.CurrentRoundSelectedPlayerCard) || GameState.Instance.Status != Assets.Scripts.GameStatus.InPlay)
        {
            return;
        }

        try
        {
            BoredGamesSocketClient.Instance.MakeMove(new MakeMoveMessage
            {
                ActionType = tag,
                GameId = GameState.Instance.GameId,
                PlayerId = GameState.Instance.PlayerId,
            });
        }
        catch (Exception ex)
        {
            Debug.LogException(ex);
            return;
        }
        
        GameState.Instance.CurrentRoundSelectedPlayerCard = tag;
    }

    void OnMouseEnter()
    {
        if (!string.IsNullOrEmpty(GameState.Instance.CurrentRoundSelectedPlayerCard) || GameState.Instance.Status != Assets.Scripts.GameStatus.InPlay)
        {
            return;
        }
        //  transform.GetComponent<SpriteRenderer>().sprite = _highlightCard;
        if (transform.position.y < defaultPosition.y)
        {
            transform.position = transform.position + new Vector3(0, popCardOffset, 0);
        }
    }

    void OnMouseExit()
    {
        if (!string.IsNullOrEmpty(GameState.Instance.CurrentRoundSelectedPlayerCard) || GameState.Instance.Status != Assets.Scripts.GameStatus.InPlay)
        {
            return;
        }
      //  transform.GetComponent<SpriteRenderer>().sprite = _card;
        if (transform.position.y > defaultPosition.y)
        {
            transform.position = defaultPosition;
        }
    }
}
