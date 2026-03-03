using System.Diagnostics;
using Godot;

namespace TickTackToe.Scripts;

public partial class PlayerNameForm : Control
{
    private Button _continueButton;
    private Control _playerNameInputControl;
    private LineEdit _playerNameInput;
    private Label _validationMessage;
    
    public override void _Ready()
    {
        _continueButton = GetNode<Button>("ContinueButton");
        _playerNameInputControl = GetNode<Control>("PlayerNameInputControl");
        _playerNameInput = _playerNameInputControl.GetNode<LineEdit>("Stack/PlayerNameInput");
        _validationMessage = _playerNameInputControl.GetNode<Label>("Stack/ValidationMessage");
        
        _playerNameInput.TextChanged += OnPlayerNameChanged;
    }
    
    private void OnPlayerNameChanged(string text)
    {
        if (_playerNameInput != null && _validationMessage != null)
        {
            if (!string.IsNullOrEmpty(_playerNameInput.Text))
            {
                _validationMessage.Hide();
                _continueButton.Disabled = false;
            }
            else
            {
                _validationMessage.Show();
                _continueButton.Disabled = true;
            }
        }
    }
}