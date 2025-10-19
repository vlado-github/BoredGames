using UnityEngine;
using UnityEngine.UI;

public class CopyInviteLinkHandler : MonoBehaviour
{
    [SerializeField] Canvas _inviteLinkDialog;
    private Button copyInviteLinkButton; // Assign in Inspector

    void Start()
    {
        GameObject copyInviteLinkButtonObject = GameObject.Find("InviteButton");
        if (copyInviteLinkButtonObject != null)
        {
            copyInviteLinkButton = copyInviteLinkButtonObject.GetComponent<Button>();
            if (copyInviteLinkButton != null)
            {
                copyInviteLinkButton.onClick.AddListener(OnButtonClick);
            }
        }
    }

    void OnButtonClick()
    {
        _inviteLinkDialog.gameObject.SetActive(true);
    }

    void OnDestroy()
    {
        copyInviteLinkButton.onClick.RemoveListener(OnButtonClick);
    }
}
