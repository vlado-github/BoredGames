using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CopyInviteLinkHandler : MonoBehaviour
{
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
        SceneManager.LoadScene("InviteScene");
    }

    void OnDestroy()
    {
        copyInviteLinkButton.onClick.RemoveListener(OnButtonClick);
    }
}
