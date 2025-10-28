using System.Collections;
using UnityEngine;

namespace Assets.Scripts.GamePlay
{
    public class PlayerHandHandler : MonoBehaviour
    {
        [SerializeField] public GameObject _playerHand;

        // Use this for initialization
        void Start()
        {
            var show = GameState.Instance.Status == GameStatus.AwaitingPlayers ||
                (GameState.Instance.Status == GameStatus.InPlay && string.IsNullOrEmpty(GameState.Instance.CurrentRoundSelectedPlayerCard));
            
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
            var show = GameState.Instance.Status == GameStatus.AwaitingPlayers || 
                (GameState.Instance.Status == GameStatus.InPlay && string.IsNullOrEmpty(GameState.Instance.CurrentRoundSelectedPlayerCard));
           
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
            if (_playerHand != null)
            {
                _playerHand.gameObject.SetActive(true);
            }
        }

        private void Hide()
        {
            if (_playerHand != null)
            {
                _playerHand.gameObject.SetActive(false);
            }
        }
    }
}