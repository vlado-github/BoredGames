using Assets.Scripts;
using Assets.Scripts.BoredGames.API;
using Assets.Scripts.BoredGames.API.Responses;
using Assets.Scripts.GamePlay;
using System.Collections;
using System.Security.Cryptography;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerNameHandler : MonoBehaviour
{
    [SerializeField] public Canvas _playerNameDialog;
    [SerializeField] public Canvas _mainMenuCanvas;
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
            GameState.Instance.PlayerName = playerName;

            StartCoroutine(BoredGamesClient.Instance.CreatePlayerProfile((response) =>
            {
                GameState.Instance.PlayerId = response.id;
                GameState.Instance.PlayerName = response.nickName;

                _playerNameDialog.gameObject.SetActive(false);
                _mainMenuCanvas.gameObject.SetActive(true);
            }));
        }
    }

    void OnDestroy()
    {
        saveButton.onClick.RemoveListener(OnButtonClick);
        _playerNameInput.onValueChanged.RemoveListener(OnInputValueChanged);
    }
}
