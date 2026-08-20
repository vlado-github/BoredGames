using System;
using System.Linq;
using Assets.Scripts.GamePlay;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OpponentProfile : MonoBehaviour
{
    [SerializeField] private Image _opponentAvatar;
    [SerializeField] private TextMeshProUGUI _opponentNickname;
    
    private void Awake()
    {
        var opponent = GameState.Instance.Players.FirstOrDefault(x => x.Id != GameState.Instance.PlayerId);
        if (opponent != null)
        {
            _opponentAvatar.color = new Color(255, 255, 255, 255);
            _opponentNickname.text = opponent.NickName;
        }
    }
}
