using System;
using Assets.Scripts.GamePlay;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerProfile : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _playerNickname;
    [SerializeField] private Image _playerAvatar;
    
    private void Awake()
    {
        if (!string.IsNullOrEmpty(GameState.Instance.PlayerName))
        {
            _playerNickname.text = GameState.Instance.PlayerName;
        }
    }
}
