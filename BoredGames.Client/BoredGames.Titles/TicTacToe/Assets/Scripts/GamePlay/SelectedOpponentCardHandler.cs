using Assets.Scripts.GamePlay;
using UnityEngine;
using UnityEngine.UI;

public class SelectedOpponentCardHandler : MonoBehaviour
{
    [SerializeField] public SpriteRenderer _selectedOpponentCardPlaceholder;
    [SerializeField] public Sprite _rock;
    [SerializeField] public Sprite _paper;
    [SerializeField] public Sprite _scissors;


    public void Show(string cardTag)
    {
        if (cardTag == "rock")
        {
            _selectedOpponentCardPlaceholder.sprite = _rock;
        }
        else if (cardTag == "paper")
        {
            _selectedOpponentCardPlaceholder.sprite = _paper;
        }
        else if (cardTag == "scissors")
        {
            _selectedOpponentCardPlaceholder.sprite = _scissors;
        }
        else
        {
            Debug.LogError($"Card tag not supported: {cardTag}");
        }
    }

    public void Hide()
    {
        _selectedOpponentCardPlaceholder.sprite = null;
    }
}
