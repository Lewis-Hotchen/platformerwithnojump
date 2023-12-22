using Godot;
using System;

public partial class PlayerJumpComponent : Node2D
{
	[Export]
	public Player Player { get; set; }

	public override void _Ready()
	{
	}

	public override void _Process(double delta)
	{
		if(Input.IsActionJustPressed("ui_accept")) {
            Player.Jump(200f);
        }

        base._Process(delta);
	}
}
