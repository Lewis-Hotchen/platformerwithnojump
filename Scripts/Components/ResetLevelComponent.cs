using System;
using Godot;

namespace PlatformerWithNoJump;

public partial class ResetLevelComponent : Node2D
{
    private EventBus eventBus;

    [Export]
    public Player Player { get; set; }

    [Export]
    public Marker2D SpawnPosition { get; set; }

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        eventBus = GetNode<EventBus>("/root/EventBus");
    }

    public void ResetLevel()
    {
        Player.Position = SpawnPosition.Position;
        GetNode<StateTracker>("/root/StateTracker").SetState(StateTracker.HasFallen, false);
        Player.Velocity = new Vector2(0, 0);
        eventBus.RaiseEvent("LevelReset", this, new EventArgs());
    }
}