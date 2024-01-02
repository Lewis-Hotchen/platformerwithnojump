using System;
using Godot;
namespace PlatformerWithNoJump;

public partial class PlayerKillBoxComponent : Node2D
{
	[Export]
	public Player Player { get; set; }

	[Export]
	public float KillFloorY { get; set; }

	[Export]
	public StateTrackerComponent StateTracker { get; set; }

	public event EventHandler<EventArgs> OnPlayerFell;


	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		 if(Player.Position.Y >= KillFloorY) {
            if(!StateTracker.States["HasFallen"]) {
                GetParent<Node2D>().GetNode<ScreenCamera>("../ScreenCamera").AddTrauma(0.5f);
                StateTracker.States["HasFallen"] = true;
                OnPlayerFell?.Invoke(this, new EventArgs());
            }
        }
	}
}
