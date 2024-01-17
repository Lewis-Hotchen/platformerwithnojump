using Godot;
namespace PlatformerWithNoJump;

public partial class TutorialPart1 : Node2D
{
    public override void _Ready()
    {
        GetNode<Timer>("MoveTimer").Timeout += OnMoveTimeout;
        base._Ready();
    }

    private StateTracker states;

    private void OnMoveTimeout()
    {
        GetNode<InGameLabel>("InGameLabel").QueueFree();
    }


    public override void _Process(double delta)
    {
        if(Input.IsActionJustPressed("right") || Input.IsActionJustPressed("left")) {
            if(!states.States["DidMove"]) {
                GetNode<Timer>("MoveTimer").Start();
                states.States["DidMove"] = true;
            }     
        }

        base._Process(delta);
    }
}
