using Assets.BoredGames.API;
using Assets.Scripts.GamePlay;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DisplayInviteLinkHandler : MonoBehaviour
{
    [SerializeField] public TMP_InputField _inviteLink;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _inviteLink.text = $"{GameConfiguration.Instance.PortalBaseUrl}&gameInstanceId={GameState.Instance.GameId}";
    }
}
