using Assets.Scripts;
using Assets.Scripts.BoredGames.API;
using Assets.Scripts.GamePlay;
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
            GameState.Instance.PlayerName = playerName;

            StartCoroutine(BoredGamesClient.Instance.GetPlayerDetails((response) =>
            {
                GameState.Instance.PlayerId = response.id;
            }));

            StartCoroutine(BoredGamesClient.Instance.JoinGame((response) =>
            {
                _validationMessage.gameObject.SetActive(false);
                _playerNameDialog.gameObject.SetActive(false);
            }));

            StartCoroutine(BoredGamesClient.Instance.GetGameState((response) => {
                GameState.Instance.Status = (GameStatus)response.gameStatus;
                GameState.Instance.CurrentRoundNumber = response.roundNumber;
                GameState.Instance.CurrentRoundStatus = response.roundStatus;

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
