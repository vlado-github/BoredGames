using Assets.Scripts.GamePlay;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.UI;

public class QuitGameHandler : MonoBehaviour
{
    private Button quitGameButton;

    [DllImport("__Internal")]
    private static extern void QuitToHome();

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
        QuitToHome(); //redirects to portal's home page 
    }

    void OnDestroy()
    {
        quitGameButton.onClick.RemoveListener(OnButtonClick);
    }
}
