using Assets.Scripts.BoredGames.API;
using System.Runtime.InteropServices;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Assets.Scripts.GamePlay
{
    public class CopyInviteHandler : MonoBehaviour
    {
        [SerializeField] public Canvas _inviteLinkCanvas;
        [SerializeField] public TMP_InputField _inviteUrlField;

        private Button copyInviteButton; // Assign in Inspector


        [DllImport("__Internal")]
        private static extern void CopyTextToClipboard(string text);

        void Start()
        {
            GameObject copyInviteButtonObject = GameObject.Find("CopyInviteButton");
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
            Debug.Log(_inviteUrlField.text);
            #if UNITY_WEBGL && !UNITY_EDITOR
                CopyTextToClipboard(_inviteUrlField.text);
            #else
                GUIUtility.systemCopyBuffer = _inviteUrlField.text;
            #endif
            _inviteLinkCanvas.gameObject.SetActive(false);
            GameManager.Instance.CheckGameStatus();
        }

        void OnDestroy()
        {
           
           copyInviteButton.onClick.RemoveListener(OnButtonClick);
        }
    }
}