using Assets.Scripts.BoredGames.API;
using Assets.Scripts.GamePlay;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayHandler : MonoBehaviour
{
    private Button playButton; // Assign in Inspector

    [SerializeField] Canvas _customMenuCanvas;
    [SerializeField] Canvas _gamePlayCanvas;
    [SerializeField] Canvas _inviteLinkDialog;

    void Start()
    {
        GameObject playButtonObject = GameObject.Find("PlayButton");
        if (playButtonObject != null)
        {
            playButton = playButtonObject.GetComponent<Button>();
            if (playButton != null)
            {
                playButton.onClick.AddListener(OnButtonClick);
            }
        }
    }

    void OnButtonClick()
    {
        StartCoroutine(BoredGamesAPIClient.Instance.CreateGame((response) =>
        {
            GameState.Instance.GameId = response.gameId;
            GameState.Instance.Status = Assets.Scripts.GameStatus.AwaitingPlayers;

            _customMenuCanvas.gameObject.SetActive(false);
            _gamePlayCanvas.gameObject.SetActive(true);
            _inviteLinkDialog.gameObject.SetActive(true);

            GameManager.Instance.CheckGameStatus();
        }));
    }

    void OnDestroy()
    {
        playButton.onClick.RemoveListener(OnButtonClick);
    }
}
