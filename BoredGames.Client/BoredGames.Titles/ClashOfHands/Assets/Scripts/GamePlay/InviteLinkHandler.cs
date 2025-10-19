using Assets.BoredGames.API;
using Assets.Scripts.GamePlay;
using TMPro;
using UnityEngine;
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
        _inviteLink.text = $"{ApiConfig.BaseUrl}game?gameInstanceId={GameState.Instance.GameId}";

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
        GUIUtility.systemCopyBuffer = _inviteLink.text;
        _inviteLinkCanvas.gameObject.SetActive(false);
        _waitingForPlayerCanvas.gameObject.SetActive(true);
    }


    // Update is called once per frame
    void Update()
    {

    }
}
