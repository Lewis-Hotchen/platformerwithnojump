using System;
using Godot;
namespace PlatformerWithNoJump;

public partial class PlayerKillBoxComponent : Node2D
{
    [Export]
    public Player Player { get; set; }

    [Export]
    public float KillFloorY { get; set; }


    public event EventHandler<EventArgs> OnPlayerFell;

    public override void _Process(double delta)
    {
        if (Player.Position.Y >= KillFloorY)
        {
            OnPlayerFell?.Invoke(this, new EventArgs());
        }
    }
}
