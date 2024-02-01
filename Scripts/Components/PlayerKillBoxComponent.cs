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
    
    private StateTracker states;

    public override void _Ready()
    {
        states = GetNode<StateTracker>("/root/StateTracker");
        states.States["HasFallen"] = false;
        base._Ready();
    }

    public override void _Process(double delta)
    {
        if (Player.Position.Y >= KillFloorY && !states.States["HasFallen"])
        {
            OnPlayerFell?.Invoke(this, new EventArgs());
            GetNode<ChumDeath>("ChumDeathLegs").Play(true);
            GetNode<ChumDeath>("ChumDeathLegs").GlobalPosition = new Vector2(Player.Position.X, KillFloorY);
            states.States["HasFallen"] = true;
            GetNode<AudioStreamPlayer2D>("DeathSound").Play();
        }
    }
}
