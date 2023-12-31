using System;
using Godot;

public partial class Springboard : Node2D
{
	[Export]
	public PlayerJumpComponent JumpComponent { get; set; }

	[Export]
	public TimerTrackerComponent Timers { get; set; }

	[Export]
	public Area2D Area { get; set; }

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Area.BodyEntered += OnBodyEntered;
		Timers.AddTimer(1, "cooldown", true);
	}

	private void OnBodyEntered(Node2D body)
	{
		if (!Timers.GetTimerRunning("cooldown"))
		{
			if (body is Player)
			{
				JumpComponent.Jump();
				GetNode<AnimationPlayer>("AnimationPlayer").Play("Extend");
				Timers.StartTimer("cooldown");
			}
		}
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
