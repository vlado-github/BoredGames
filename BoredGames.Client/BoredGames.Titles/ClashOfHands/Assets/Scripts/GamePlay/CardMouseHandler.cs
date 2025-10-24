using Assets.Scripts.BoredGames.API;
using Assets.Scripts.GamePlay;
using System;
using Unity.VisualScripting;
using UnityEngine;

public class CardMouseHandler : MonoBehaviour
{
    [SerializeField] Sprite _card;
    [SerializeField] Sprite _highlightCard;
    [SerializeField] float popCardOffset;

    void OnMouseDown()
    {
        if (GameState.Instance.CurrentRoundCardSelected || GameState.Instance.Status != Assets.Scripts.GameStatus.InPlay)
        {
            return;
        }

        Debug.Log($"OnMouseDown {tag}");

        var isSuccess = true;
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
            isSuccess = false;
            Debug.LogException(ex);
        }

        GameState.Instance.CurrentRoundCardSelected = isSuccess;
    }

    void OnMouseEnter()
    {
        if (GameState.Instance.CurrentRoundCardSelected || GameState.Instance.Status != Assets.Scripts.GameStatus.InPlay)
        {
            return;
        }
        Debug.Log("Mouse entered!");
        transform.GetComponent<SpriteRenderer>().sprite = _highlightCard;
        transform.position = transform.position + new Vector3(0, popCardOffset, 0);
    }

    void OnMouseExit()
    {
        if (GameState.Instance.CurrentRoundCardSelected || GameState.Instance.Status != Assets.Scripts.GameStatus.InPlay)
        {
            return;
        }
        Debug.Log("Mouse exited!");
        transform.GetComponent<SpriteRenderer>().sprite = _card;
        transform.position = transform.position + new Vector3(0, -popCardOffset, 0);
    }
}
