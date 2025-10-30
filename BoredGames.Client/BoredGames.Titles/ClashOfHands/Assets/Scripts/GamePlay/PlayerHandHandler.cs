using System.Collections;
using UnityEngine;

namespace Assets.Scripts.GamePlay
{
    public class PlayerHandHandler : MonoBehaviour
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