using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class QuickPlayHandler : MonoBehaviour
{
    private Button quickPlayButton; // Assign in Inspector

    void Start()
    {
        GameObject quickPlayButtonObject = GameObject.Find("QuickPlayButton");
        if (quickPlayButtonObject != null)
        {
            quickPlayButton = quickPlayButtonObject.GetComponent<Button>();
            if (quickPlayButton != null)
            {
                quickPlayButton.onClick.AddListener(OnButtonClick);
            }
        }
    }

    void OnButtonClick()
    {
        SceneManager.LoadScene("GamePlayScene");
    }

    void OnDestroy()
    {
        quickPlayButton.onClick.RemoveListener(OnButtonClick);
    }
}
