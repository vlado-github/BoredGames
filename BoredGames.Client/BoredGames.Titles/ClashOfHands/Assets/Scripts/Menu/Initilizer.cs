using Assets.Scripts.GamePlay;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.Menu
{
    public class Initilizer : MonoBehaviour
    {

        // Use this for initialization
        private void Awake()
        {
            if (GameState.Instance.Status == GameStatus.InPlay)
            {
                SceneManager.LoadScene("GamePlayScene");
            }
        }
    }
}