using Godot;
using PlatformerWithNoJump;
using System;

public partial class Exit : Node2D
{
    public override void _Ready()
    {
        base._Ready();
    }

    private void OnBodyEntered(Node2D body) {

        if(body is Player p) {
            GD.Print("Entered exit");
            CallDeferred(nameof(GoToNext));
        }
    }

    private void GoToNext() {
        GetParent<BaseLevel>().NextLevel();
    }
}
