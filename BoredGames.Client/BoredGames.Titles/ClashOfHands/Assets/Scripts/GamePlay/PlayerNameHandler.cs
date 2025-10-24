using Assets.Scripts;
using Assets.Scripts.BoredGames.API;
using Assets.Scripts.GamePlay;
using System;
using System.Collections.Generic;
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
    public static string GameInstanceIdParamKey = "gameInstanceId";

    private void Awake()
    {
        Dictionary<string, string> queryParams = Utils.GetQueryParams();

        if (queryParams.ContainsKey(GameInstanceIdParamKey))
        {
            string value = queryParams[GameInstanceIdParamKey];
            if (string.IsNullOrEmpty(value))
            {
                return;
            }
            GameState.Instance.GameId = value;
        }
    }

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

            StartCoroutine(BoredGamesAPIClient.Instance.CreatePlayerProfile((response) =>
            {
                GameState.Instance.PlayerId = response.id;
                GameState.Instance.PlayerName = response.nickName;

                if (GameState.Instance.IsGameCreated)
                {
                    Debug.Log($"JoinGame {response.nickName}");
                    StartCoroutine(BoredGamesAPIClient.Instance.JoinGame((response) => { }));
                    _playerNameDialog.gameObject.SetActive(false);
                    _mainMenuCanvas.gameObject.SetActive(false);

                    GameManager.Instance.CheckGameStatus();
                }
                else
                {
                    Debug.Log($"JoinGame {GameState.Instance.IsGameCreated}");
                    _playerNameDialog.gameObject.SetActive(false);
                    _mainMenuCanvas.gameObject.SetActive(true);
                }
            }));
        }
    }

    void OnDestroy()
    {
        saveButton.onClick.RemoveListener(OnButtonClick);
        _playerNameInput.onValueChanged.RemoveListener(OnInputValueChanged);
    }
}
