using Assets.Scripts.GamePlay;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class InviteLinkHandler : MonoBehaviour
{
    private Button copyInviteButton;

    [SerializeField] public TextMeshProUGUI _inviteLink;
    [SerializeField] public Canvas _inviteLinkCanvas;
    [SerializeField] public Canvas _waitingForPlayerCanvas;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _inviteLink.text = $"{GameConfiguration.Instance.BaseUrl}/game?gameInstanceId={GameState.Instance.GameId}";

        GameObject copyInviteButtonObject = GameObject.Find("CopyLinkButton");
        if (copyInviteButtonObject != null)
        {
            copyInviteButton = copyInviteButtonObject.GetComponent<Button>();
            if (copyInviteButton != null)
            {
                copyInviteButton.onClick.AddListener(OnButtonClick);
            }
        }
    }

    void OnButtonClick()
    {
        GUIUtility.systemCopyBuffer = $"{GameConfiguration.Instance.BaseUrl}/game?gameInstanceId={GameState.Instance.GameId}";
        _inviteLinkCanvas.gameObject.SetActive(false);
        _waitingForPlayerCanvas.gameObject.SetActive(true);
    }


    // Update is called once per frame
    void Update()
    {

    }
}
