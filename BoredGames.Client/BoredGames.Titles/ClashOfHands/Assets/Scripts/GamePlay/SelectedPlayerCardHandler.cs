using Assets.Scripts.GamePlay;
using UnityEngine;
using UnityEngine.UI;

public class SelectedPlayerCardHandler : MonoBehaviour
{
    [SerializeField] public RawImage _image;
    [SerializeField] public Texture _rock;
    [SerializeField] public Texture _paper;
    [SerializeField] public Texture _scissors;

    void Start()
    {
        Hide();
    }

    void Update()
    {
        if (!string.IsNullOrEmpty(GameState.Instance.CurrentRoundSelectedPlayerCard))
        {
            Show();
        }
        else
        {
            Hide();
        }
    }

    private void Show()
    {
        if (_image != null)
        {
            if (GameState.Instance.CurrentRoundSelectedPlayerCard == "rock")
            {
                _image.texture = _rock;
            }
            else if (GameState.Instance.CurrentRoundSelectedPlayerCard == "paper")
            {
                _image.texture = _paper;
            }
            else
            {
                _image.texture = _scissors;
            }
            _image.color = new Color(255, 255, 255, 255);
        }
    }

    private void Hide()
    {
        if (_image != null)
        {
            _image.texture = null;
            _image.color = new Color(255, 255, 255, 0);
        }
    }
}
