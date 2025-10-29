using UnityEngine;
using UnityEngine.UI;

public class BackHandler : MonoBehaviour
{
    [SerializeField] public Canvas _mainMenuCanvas;
    [SerializeField] public Canvas _customMenuCanvas;

    private Button backButton; // Assign in Inspector

    void Start()
    {
        GameObject backButtonObject = GameObject.Find("BackButton");
        if (backButtonObject != null)
        {
            backButton = backButtonObject.GetComponent<Button>();
            if (backButton != null)
            {
                backButton.onClick.AddListener(OnButtonClick);
            }
        }
    }

    void OnButtonClick()
    {
        _customMenuCanvas.gameObject.SetActive(false);
        _mainMenuCanvas.gameObject.SetActive(true);
    }

    void OnDestroy()
    {
        backButton.onClick.RemoveListener(OnButtonClick);
    }
}
