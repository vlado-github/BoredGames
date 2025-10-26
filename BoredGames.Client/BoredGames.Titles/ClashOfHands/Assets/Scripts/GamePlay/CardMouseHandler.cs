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
    [SerializeField] string cardsInHand;
    [SerializeField] GameManager _gameManager;

    private Vector3 defaultPosition;
    private bool isSelected = false;

    private void Start()
    {
        defaultPosition = transform.position;
    }

    void OnMouseDown()
    {
        Debug.Log($"Click: {tag}");
        if (isSelected || GameState.Instance.CurrentRoundCardSelected || GameState.Instance.Status != Assets.Scripts.GameStatus.InPlay)
        {
            Debug.Log($"Click: {isSelected}");
            return;
        }

        try
        {
            Debug.Log($"Click: MakeMove");
            BoredGamesSocketClient.Instance.MakeMove(new MakeMoveMessage
            {
                ActionType = tag,
                GameId = GameState.Instance.GameId,
                PlayerId = GameState.Instance.PlayerId,
            });
        }
        catch (Exception ex)
        {
            isSelected = false;
            Debug.LogException(ex);
        }
        
        isSelected = true;
        GameState.Instance.CurrentRoundCardSelected = isSelected;
        Debug.Log($"Click: DisplayPlayerSide");
        _gameManager.DisplayPlayerSide(show: false);
        Debug.Log($"Click: ShowSelectedCard");
        _gameManager.ShowSelectedCard(tag, isOppenent: false);
    }

    void OnMouseEnter()
    {
        if (isSelected || GameState.Instance.CurrentRoundCardSelected || GameState.Instance.Status != Assets.Scripts.GameStatus.InPlay)
        {
            return;
        }
        transform.GetComponent<SpriteRenderer>().sprite = _highlightCard;
        if (transform.position.y < defaultPosition.y)
        {
            transform.position = transform.position + new Vector3(0, popCardOffset, 0);
        }
    }

    void OnMouseExit()
    {
        if (isSelected || GameState.Instance.CurrentRoundCardSelected || GameState.Instance.Status != Assets.Scripts.GameStatus.InPlay)
        {
            return;
        }
        transform.GetComponent<SpriteRenderer>().sprite = _card;
        if (transform.position.y > defaultPosition.y)
        {
            transform.position = transform.position + new Vector3(0, -popCardOffset, 0);
        }
    }
}
