using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.GamePlay
{
    public class CloseInviteHandler : MonoBehaviour
    {
        [SerializeField] public Canvas _inviteLinkCanvas;

        private Button closeInviteButton; // Assign in Inspector

        void Start()
        {
            GameObject closeInviteButtonObject = GameObject.Find("CloseInviteButton");
            if (closeInviteButtonObject != null)
            {
                closeInviteButton = closeInviteButtonObject.GetComponent<Button>();
                if (closeInviteButton != null)
                {
                    closeInviteButton.onClick.AddListener(OnButtonClick);
                }
            }
        }

        void OnButtonClick()
        {
            _inviteLinkCanvas.gameObject.SetActive(true);
        }

        void OnDestroy()
        {
            closeInviteButton.onClick.RemoveListener(OnButtonClick);
        }
    }
}