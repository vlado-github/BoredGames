using Assets.Scripts.GamePlay;
using TMPro;
using UnityEngine;

public class PlayerProfile : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _playerNickname;
    
    private void Awake()
    {
        if (!string.IsNullOrEmpty(GameState.Instance.PlayerName))
        {
            _playerNickname.text = GameState.Instance.PlayerName;
        }
    }
}
