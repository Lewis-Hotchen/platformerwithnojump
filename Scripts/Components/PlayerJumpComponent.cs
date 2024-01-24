using Godot;
using System;
namespace PlatformerWithNoJump;

public partial class PlayerJumpComponent : Node2D
{
    [Export]
    public Player Player { get; set; }

    [Export]
    public float Force { get; set; }

    public override void _Process(double delta)
    {
        if (Input.IsActionJustPressed("ui_accept"))
        {
            if (Player.IsOnFloor)
            {
                Player.ApplyImpulse(Vector2.Up * Force);
            }
        }

        base._Process(delta);
    }
}
