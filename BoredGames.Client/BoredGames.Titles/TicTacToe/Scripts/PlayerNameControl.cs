using Godot;
using System;
using System.Diagnostics;

public delegate void OnPlayerNameChanged(string test);

public partial class PlayerNameControl : Control
{
	public event OnPlayerNameChanged PlayerNameChanged;
	
	private LineEdit _playerNameInput;
	private Label _validationMessage;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_playerNameInput = GetNode<LineEdit>("PlayerNameInput");
		_validationMessage = GetNode<Label>("ValidationMessage");
		
		_playerNameInput.TextChanged += OnPlayerNameChanged;
	}

	private void OnPlayerNameChanged(string text)
	{
		if (_playerNameInput != null && _validationMessage != null)
		{
			if (!string.IsNullOrEmpty(_playerNameInput.Text))
			{
				_validationMessage.Hide();
				PlayerNameChanged?.Invoke(_playerNameInput.Text);
			}
			else
			{
				_validationMessage.Show();
				PlayerNameChanged?.Invoke(string.Empty);
			}
		}
	}
}
