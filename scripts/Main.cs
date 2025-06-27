using Godot;
using System;

public partial class Main : Node
{
	[Export]
	public PackedScene MobScene;

	private int _score;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	private void _game_over()
	{
		GetNode<Timer>("MobTimer").Stop();
		GetNode<Timer>("ScoreTimer").Stop();
		
		GetNode<Hud>("HUD").ShowGameOver();
		
		GetNode<AudioStreamPlayer2D>("Music").Stop();
		GetNode<AudioStreamPlayer>("DeathSound").Play();

	}

	private void _new_game()
	{
		_score = 0;
		GetNode<AudioStreamPlayer2D>("Music").Play();
		
		var player = GetNode<Player>("Player");
		var startPosition = GetNode<Marker2D>("StartPosition");
		player.Start(startPosition.Position);
		
		GetNode<Timer>("StartTimer").Start();
		
		var hud = GetNode<Hud>("HUD");
		hud.UpdateScore(_score);
		hud.showMessage("Get Ready!");
		
		GetTree().CallGroup("mobs", Node.MethodName.QueueFree);

	}

	private void _on_score_timer_timeout()
	{
		_score++;
		GetNode<Hud>("HUD").UpdateScore(_score);
	}

	private void _on_start_timer_timeout()
	{
		GetNode<Timer>("MobTimer").Start();
		GetNode<Timer>("ScoreTimer").Start();
	}

	private void _on_mob_timer_timeout()
	{
		Mob mob = MobScene.Instantiate<Mob>();
		var mobSpawnLocation = GetNode<PathFollow2D>("MobPath/MobSpawnLocation");
		mobSpawnLocation.ProgressRatio = GD.Randf();
		
		var direction = mobSpawnLocation.Rotation + Mathf.Pi / 2;

		
		mob.Position = mobSpawnLocation.Position;

		direction += (float)GD.RandRange(-Mathf.Pi / 4, Mathf.Pi / 4);
		mob.Rotation = direction;
		
		var velocity = new Vector2((float)GD.RandRange(150.0f, 250.0f), 0);
		mob.LinearVelocity = velocity.Rotated(direction);
		
		AddChild(mob);
	}
}
