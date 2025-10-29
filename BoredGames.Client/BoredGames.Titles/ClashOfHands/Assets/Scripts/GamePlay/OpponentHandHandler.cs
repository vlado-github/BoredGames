using System.Collections;
using UnityEngine;

namespace Assets.Scripts.GamePlay
{
    public class OpponentHandHandler : MonoBehaviour
    {
        public void Show()
        {
            Debug.Log("OpponentHand show");
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            Debug.Log("OpponentHand hide");
            gameObject.SetActive(false);
        }
    }
}