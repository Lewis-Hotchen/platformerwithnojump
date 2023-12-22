using Godot;

public partial class ResetLevelComponent : Node2D
{
	[Export]
	public StateTrackerComponent StateTracker { get; set; }

	[Export]
	public Player Player { get; set; }

	[Export]
	public Vector2 StartPosition { get; set; }

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void ResetLevel() {
		Player.Position = StartPosition;
        StateTracker.States["PlayerHasFallen"] = false;
	}
}
