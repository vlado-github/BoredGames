using Assets.Scripts.GamePlay;
using UnityEngine;
using UnityEngine.UI;

public class SelectedPlayerCardHandler : MonoBehaviour
{
    [SerializeField] public SpriteRenderer _selectedPlayerCardPlaceholder;
    [SerializeField] public Sprite _rock;
    [SerializeField] public Sprite _paper;
    [SerializeField] public Sprite _scissors;

    public void Show(string cardTag)
    {
        if (cardTag == "rock")
        {
            _selectedPlayerCardPlaceholder.sprite = _rock;
        }
        else if (cardTag == "paper")
        {
            _selectedPlayerCardPlaceholder.sprite = _paper;
        }
        else if (cardTag == "scissors")
        {
            _selectedPlayerCardPlaceholder.sprite = _scissors;
        }
        else
        {
            Debug.LogError($"Card tag not supported: {cardTag}");
        }

    }

    public void Hide()
    {
        _selectedPlayerCardPlaceholder.sprite = null;
    }
}
