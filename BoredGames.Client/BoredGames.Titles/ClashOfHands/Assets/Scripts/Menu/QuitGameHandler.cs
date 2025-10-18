using UnityEngine;
using UnityEngine.UI;

public class QuitGameHandler : MonoBehaviour
{
    private Button quitGameButton;

    void Start()
    {
        GameObject quitGameButtonObject = GameObject.Find("QuitButton");
        if (quitGameButtonObject != null)
        {
            quitGameButton = quitGameButtonObject.GetComponent<Button>();
            if (quitGameButton != null)
            {
                quitGameButton.onClick.AddListener(OnButtonClick);
            }
        }
    }

    void OnButtonClick()
    {
        Debug.Log("Quit game");
        Application.Quit();
    }

    void OnDestroy()
    {
        quitGameButton.onClick.RemoveListener(OnButtonClick);
    }
}
