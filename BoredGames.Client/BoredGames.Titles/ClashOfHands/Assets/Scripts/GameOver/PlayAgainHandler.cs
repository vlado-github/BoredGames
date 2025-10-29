using Assets.Scripts;
using Assets.Scripts.BoredGames.API;
using Assets.Scripts.GamePlay;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayAgainHandler : MonoBehaviour
{
    private Button playAgainButton;

    void Start()
    {
        GameObject playAgainButtonObject = GameObject.Find("PlayAgainButton");
        if (playAgainButtonObject != null)
        {
            playAgainButton = playAgainButtonObject.GetComponent<Button>();
            if (playAgainButton != null)
            {
                playAgainButton.onClick.AddListener(OnButtonClick);
            }
        }
    }

    void OnButtonClick()
    {
        //StartCoroutine(BoredGamesAPIClient.Instance.PlayAgain((response) =>
        //{
            
        //}));
    }

    void OnDestroy()
    {
        playAgainButton.onClick.RemoveListener(OnButtonClick);
    }
}
