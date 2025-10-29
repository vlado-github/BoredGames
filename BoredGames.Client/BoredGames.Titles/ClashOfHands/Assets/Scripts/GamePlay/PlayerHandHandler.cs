using System.Collections;
using UnityEngine;

namespace Assets.Scripts.GamePlay
{
    public class PlayerHandHandler : MonoBehaviour
    {
        public void Show()
        {
            Debug.Log("PlayerHand show");
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            Debug.Log("PlayerHand hide");
            gameObject.SetActive(false);
        }
    }
}