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
                && GameState.Instance.IsRoundResultDisplayCompleted();
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
                && GameState.Instance.IsRoundResultDisplayCompleted();
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
                //foreach (var gameobject in GameObject.FindGameObjectsWithTag("opponent_hand"))
                //{
                //    gameobject.SetActive(true);
                //}
            }
        }

        private void Hide()
        {
            if (_opponentHand != null)
            {
                _opponentHand.gameObject.SetActive(false);
                //foreach (var gameobject in GameObject.FindGameObjectsWithTag("opponent_hand"))
                //{
                //    gameobject.SetActive(false);
                //}
            }
        }
    }
}