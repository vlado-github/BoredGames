using UnityEngine;
using UnityEngine.UI;

public class ShowInviteLinkHandler : MonoBehaviour
{
    [SerializeField] Canvas _inviteLinkDialog;
    private Button showInviteLinkButton; // Assign in Inspector

    void Start()
    {
        GameObject showInviteLinkButtonObject = GameObject.Find("InviteButton");
        if (showInviteLinkButtonObject != null)
        {
            showInviteLinkButton = showInviteLinkButtonObject.GetComponent<Button>();
            if (showInviteLinkButton != null)
            {
                showInviteLinkButton.onClick.AddListener(OnButtonClick);
            }
        }
    }

    void OnButtonClick()
    {
        _inviteLinkDialog.gameObject.SetActive(true);
    }

    void OnDestroy()
    {
        showInviteLinkButton.onClick.RemoveListener(OnButtonClick);
    }
}
