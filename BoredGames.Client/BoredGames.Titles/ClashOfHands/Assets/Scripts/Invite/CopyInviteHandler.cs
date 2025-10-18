using Assets.Scripts.GamePlay;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CopyInviteHandler : MonoBehaviour
{
    private Button copyInviteButton;

    [SerializeField] public TextMeshProUGUI _inviteLink;
    


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _inviteLink.text = $"{GameConfiguration.Instance.BaseUrl}/game?gameInstanceId={GameState.Instance.Id}";

        GameObject copyInviteButtonObject = GameObject.Find("CopyButton");
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
        GUIUtility.systemCopyBuffer = $"{GameConfiguration.Instance.BaseUrl}/game?gameInstanceId={GameState.Instance.Id}";
        SceneManager.LoadScene("GamePlayScene");
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
