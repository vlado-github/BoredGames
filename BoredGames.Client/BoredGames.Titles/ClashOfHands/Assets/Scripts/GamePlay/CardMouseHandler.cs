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
    [SerializeField] float centerX;
    [SerializeField] float centerY;
    [SerializeField] float centerZ;

    private Vector3 defaultPosition;
    private Quaternion defaultRotation;

    private void Start()
    {
        defaultPosition = transform.position;
        defaultRotation = transform.rotation;
    }

    void OnMouseDown()
    {
        if (GameState.Instance.CurrentRoundCardSelected || GameState.Instance.Status != Assets.Scripts.GameStatus.InPlay)
        {
            return;
        }

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
        HideOtherCardsInHand();
        Center();
    }

    void OnMouseEnter()
    {
        if (GameState.Instance.CurrentRoundCardSelected || GameState.Instance.Status != Assets.Scripts.GameStatus.InPlay)
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
        if (GameState.Instance.CurrentRoundCardSelected || GameState.Instance.Status != Assets.Scripts.GameStatus.InPlay)
        {
            return;
        }
        transform.GetComponent<SpriteRenderer>().sprite = _card;
        if (transform.position.y > defaultPosition.y)
        {
            transform.position = transform.position + new Vector3(0, -popCardOffset, 0);
        }
    }

    private void HideOtherCardsInHand()
    {
        var cards = cardsInHand.Split(",");
        foreach (var card in cards.Where(x => !CompareTag(x)))
        {
            var cardObject = GameObject.FindGameObjectWithTag(card);
            cardObject.SetActive(false);
        }
    }

    private void Center()
    {
        transform.position = new Vector3(centerX, centerY, centerZ);
        transform.rotation = Quaternion.Euler(0, 0, 0);
    }
}
