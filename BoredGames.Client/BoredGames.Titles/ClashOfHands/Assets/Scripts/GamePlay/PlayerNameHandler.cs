using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using Assets.Scripts.GamePlay;
using Unity.VisualScripting;

public class PlayerNameHandler : MonoBehaviour
{
    [SerializeField] public Canvas _playerNameDialog;
    [SerializeField] public Canvas _inviteLinkDialog;
    [SerializeField] public TMP_InputField _playerNameInput;
    [SerializeField] public TextMeshProUGUI _validationMessage;
    private Button saveButton; 

    void Start()
    {
        GameObject saveButtonObject = GameObject.Find("SaveButton");
        if (saveButtonObject != null)
        {
            saveButton = saveButtonObject.GetComponent<Button>();
            if (saveButton != null)
            {
                saveButton.onClick.AddListener(OnButtonClick);
            }
        }

        _playerNameInput.onValueChanged.AddListener(OnInputValueChanged);
    }

    void OnInputValueChanged(string text)
    {
        if (!string.IsNullOrEmpty(text))
        {
            _validationMessage.gameObject.SetActive(false);
        }
        else
        {
            _validationMessage.gameObject.SetActive(true);
        }
    }

    void OnButtonClick()
    {
        var playerName = _playerNameInput.text;
        if (string.IsNullOrEmpty(playerName))
        {
            _validationMessage.gameObject.SetActive(true);
        }
        else
        {
            _validationMessage.gameObject.SetActive(false);
            GameState.Instance.PlayerName = playerName;
            _playerNameDialog.gameObject.SetActive(false);
            _inviteLinkDialog.gameObject.SetActive(true);
        }
    }

    void OnDestroy()
    {
        saveButton.onClick.RemoveListener(OnButtonClick);
    }
}
