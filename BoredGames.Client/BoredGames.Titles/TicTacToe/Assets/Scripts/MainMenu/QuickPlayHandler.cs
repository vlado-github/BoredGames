using Assets.Scripts;
using Assets.Scripts.BoredGames.API;
using Assets.Scripts.GamePlay;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class QuickPlayHandler : MonoBehaviour
{
    private Button quickPlayButton; // Assign in Inspector

    [SerializeField] Canvas _mainMenuCanvas;
    [SerializeField] Canvas _inviteLinkDialog;

    void Start()
    {
        GameObject quickPlayButtonObject = GameObject.Find("QuickPlayButton");
        if (quickPlayButtonObject != null)
        {
            quickPlayButton = quickPlayButtonObject.GetComponent<Button>();
            if (quickPlayButton != null)
            {
                quickPlayButton.onClick.AddListener(OnButtonClick);
            }
        }
    }

    void OnButtonClick()
    {
        StartCoroutine(BoredGamesAPIClient.Instance.CreateGame((response) =>
        {
            GameState.Instance.GameId = response.gameId;
            GameState.Instance.Status = GameStatus.AwaitingPlayers;

            if (!BoredGamesSocketClient.Instance.IsConnected())
            {
                BoredGamesSocketClient.Instance.SetupConnection();
            }

            _mainMenuCanvas.gameObject.SetActive(false);
            _inviteLinkDialog.gameObject.SetActive(true);

            GameManager.Instance.CheckGameStatus();
        }));
    }

    void OnDestroy()
    {
        quickPlayButton.onClick.RemoveListener(OnButtonClick);
    }
}
