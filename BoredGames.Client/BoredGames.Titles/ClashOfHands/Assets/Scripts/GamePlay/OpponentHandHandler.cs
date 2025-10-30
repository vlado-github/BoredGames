using System.Collections;
using UnityEngine;

namespace Assets.Scripts.GamePlay
{
    public class OpponentHandHandler : MonoBehaviour
    {
        public void Show()
        {
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }
    }
}