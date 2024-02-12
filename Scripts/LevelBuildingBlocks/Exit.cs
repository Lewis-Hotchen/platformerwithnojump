using Godot;
using PlatformerWithNoJump;
using System;

public partial class Exit : Node2D
{

    [Export]
    public NextLevelComponent NextLevelComponent { get; set; }

    public override void _Ready()
    {
        base._Ready();
    }

    private void OnBodyEntered(Node2D body)
    {

        if (body is Player p)
        {
            CallDeferred(nameof(GoToNext));
        }
    }

    private void GoToNext()
    {
        NextLevelComponent.NextLevel();
    }
}
