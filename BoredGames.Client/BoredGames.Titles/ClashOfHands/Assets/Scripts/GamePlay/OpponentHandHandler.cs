using System.Collections;
using UnityEngine;

namespace Assets.Scripts.GamePlay
{
    public class OpponentHandHandler : MonoBehaviour
    {
        [SerializeField] public GameObject _opponentHand;

        // Use this for initialization
        void Start()
        {
            var show = GameState.Instance.Status == GameStatus.InPlay 
                && string.IsNullOrEmpty(GameState.Instance.CurrentRoundSelectedOpponentCard);
            if (show)
            {
                Show();
            }
            else
            {
                Hide();
            }
        }

        // Update is called once per frame
        void Update()
        {
            var show = GameState.Instance.Status == GameStatus.InPlay
                && string.IsNullOrEmpty(GameState.Instance.CurrentRoundSelectedOpponentCard);
            if (show)
            {
                Show();
            }
            else
            {
                Hide();
            }
        }

        private void Show()
        {
            if (_opponentHand != null)
            {
                _opponentHand.gameObject.SetActive(true);
            }
        }

        private void Hide()
        {
            if (_opponentHand != null)
            {
                _opponentHand.gameObject.SetActive(false);
            }
        }
    }
}