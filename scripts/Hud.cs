using Godot;
using System;

public partial class Hud : CanvasLayer
{
	
	[Signal]
	public delegate void StartGameEventHandler();

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void showMessage(string text)
	{
		var message = GetNode<Label>("Message");
		message.Text = text;
		message.Show();
		
		GetNode<Timer>("MessageTimer").Start();
	}

	async public void ShowGameOver()
	{
		showMessage("Game Over");

		var MessageTimer = GetNode<Timer>("MessageTimer");
		await ToSignal(MessageTimer, Timer.SignalName.Timeout);
		
		var message = GetNode<Label>("Message");
		message.Text = "Dodge the creeps";
		message.Show();
		
		await ToSignal(GetTree().CreateTimer(1.0), SceneTreeTimer.SignalName.Timeout);
		GetNode<Button>("StartButton").Show();
	}

	public void UpdateScore(int score)
	{
		GetNode<Label>("ScoreLabel").Text = score.ToString();
	}

	private void _on_start_button_pressed()
	{
		GetNode<Button>("StartButton").Hide();
		EmitSignal(SignalName.StartGame);
	}

	private void _on_message_timer_timeout()
	{
		GetNode<Label>("Message").Hide();
	}

	
}
