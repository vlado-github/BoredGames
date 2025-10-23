using Assets.Scripts.BoredGames.API;
using Assets.Scripts.GamePlay;
using Unity.VisualScripting;
using UnityEngine;

public class CardClickHandler : MonoBehaviour
{
    [SerializeField] Sprite _card;
    [SerializeField] Sprite _highlightCard;

    void OnMouseEnter()
    {
        Debug.Log($"OnMouseEnter: {_card.GameObject().name}");
        transform.GetComponent<SpriteRenderer>().sprite = _highlightCard;
    }

    void OnMouseExit()
    {
        Debug.Log($"OnMouseExit: {_card.GameObject().name}");
        transform.GetComponent<SpriteRenderer>().sprite = _card;
    }

    void OnMouseDown()
    {
        Debug.Log($"OnMouseDown: {_card.GameObject().name}");
        if (!GameState.Instance.IsGameCreated)
        {
            return;
        }

        BoredGamesSocketClient.MakeMove(GameState.Instance.GameId, new MakeMoveMessage
        {
            gameId = GameState.Instance.GameId,
            playerId = GameState.Instance.PlayerId,
            actionType = _card.GameObject().name,
        });
    }
}
