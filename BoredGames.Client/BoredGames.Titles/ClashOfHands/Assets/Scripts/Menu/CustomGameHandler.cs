using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CustomGameHandler : MonoBehaviour
{
    private Button customGameButton; // Assign in Inspector

    void Start()
    {
        GameObject customGameButtonObject = GameObject.Find("CustomGameButton");
        if (customGameButtonObject != null)
        {
            customGameButton = customGameButtonObject.GetComponent<Button>();
            if (customGameButton != null)
            {
                customGameButton.onClick.AddListener(OnButtonClick);
            }
        }
    }

    void OnButtonClick()
    {
        SceneManager.LoadScene("SetupScene");
    }

    void OnDestroy()
    {
        customGameButton.onClick.RemoveListener(OnButtonClick);
    }
}
