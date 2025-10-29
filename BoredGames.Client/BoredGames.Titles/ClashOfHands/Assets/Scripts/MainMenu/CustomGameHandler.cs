using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CustomGameHandler : MonoBehaviour
{
    private Button customGameButton;

    [SerializeField] Canvas _mainMenuCanvas;
    [SerializeField] Canvas _customMenuCanvas;

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
        _mainMenuCanvas.gameObject.SetActive(false);
        _customMenuCanvas.gameObject.SetActive(true);
    }

    void OnDestroy()
    {
        customGameButton.onClick.RemoveListener(OnButtonClick);
    }
}
