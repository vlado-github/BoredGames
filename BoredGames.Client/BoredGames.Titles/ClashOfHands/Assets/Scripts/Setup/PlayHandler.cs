using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayHandler : MonoBehaviour
{
    private Button playButton; // Assign in Inspector

    void Start()
    {
        GameObject playButtonObject = GameObject.Find("PlayButton");
        if (playButtonObject != null)
        {
            playButton = playButtonObject.GetComponent<Button>();
            if (playButton != null)
            {
                playButton.onClick.AddListener(OnButtonClick);
            }
        }
    }

    void OnButtonClick()
    {
        //GameManager.Instance.CreateGame();


        SceneManager.LoadScene("GamePlayScene");
    }

    void OnDestroy()
    {
        playButton.onClick.RemoveListener(OnButtonClick);
    }
}
