using Godot;
using System;

public enum GameState { Playing, Paused }

public partial class GameStateManager : Node
{
	public static GameStateManager Instance;

	public GameState CurrentState { get; private set; } = GameState.Playing;

	public override void _Ready()
	{
		Instance = this;
		Input.MouseMode = Input.MouseModeEnum.Captured;
		ProcessMode = ProcessModeEnum.Always;
	}
	
   	public override void _Process(double delta)
	{
		if (Input.IsActionJustPressed("ui_cancel"))
		{
			TogglePause();
		}
	}
	
	public void TogglePause()
	{
		// DEBUG
		//GD.Print("ESC input.");
		
		if (CurrentState == GameState.Playing)
			Pause();
		else
			Resume();
	}

	public void Pause()
	{
		// DEBUG
		GD.Print("Paused.");
		
		CurrentState = GameState.Paused;
		GetTree().Paused = true;
		Input.MouseMode = Input.MouseModeEnum.Visible;
	}

	public void Resume()
	{
		// DEBUG
		GD.Print("Resumed.");
		
		CurrentState = GameState.Playing;
		GetTree().Paused = false;
		Input.MouseMode = Input.MouseModeEnum.Captured;
	}
}
