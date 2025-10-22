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
            Debug.LogWarning("[client] playernamehandler - OnButtonClick: " + GameState.Instance.GameId);
            GameState.Instance.PlayerName = playerName;

            StartCoroutine(BoredGamesClient.Instance.GetPlayerDetails((response) =>
            {
                Debug.LogWarning("[client] GetPlayerDetails: " + GameState.Instance.GameId);
                GameState.Instance.PlayerId = response.id;
            }));

            StartCoroutine(BoredGamesClient.Instance.JoinGame((response) =>
            {
                Debug.LogWarning("[client] JoinGame: " + GameState.Instance.GameId);
            }));

            StartCoroutine(BoredGamesClient.Instance.GetGameState((response) => {
                GameState.Instance.Status = (GameStatus)response.gameStatus;
                GameState.Instance.CurrentRoundNumber = response.roundNumber;
                GameState.Instance.CurrentRoundStatus = response.roundStatus;


                Debug.LogWarning("[client] GameId: " + GameState.Instance.GameId);
                Debug.LogWarning("[client] GameStatus: " + GameState.Instance.Status);
                Debug.LogWarning("[client] RoundStatus: " + GameState.Instance.CurrentRoundStatus);
                Debug.LogWarning("[client] RoundNumber: " + GameState.Instance.CurrentRoundNumber);
                Debug.LogWarning("[client] GameStatus: " + GameState.Instance.Status);

                _validationMessage.gameObject.SetActive(false);
                _playerNameDialog.gameObject.SetActive(false);

                if (GameState.Instance.Status == GameStatus.AwaitingPlayers)
                {
                    _inviteLinkDialog.gameObject.SetActive(true);
                }
                else
                {
                    _inviteLinkDialog.gameObject.SetActive(false);
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
