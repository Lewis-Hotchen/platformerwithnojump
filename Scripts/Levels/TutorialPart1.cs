using Godot;
namespace PlatformerWithNoJump;

public partial class TutorialPart1 : Node2D
{
    [Export]
    public BaseLevel Level { get; set; }

    public override void _Ready()
    {
        GetNode<Timer>("MoveTimer").Timeout += OnMoveTimeout;
        base._Ready();
    }

    private void OnMoveTimeout()
    {
        GetNode<InGameLabel>("InGameLabel").QueueFree();
    }


    public override void _Process(double delta)
    {
        if(Input.IsActionJustPressed("ui_right") || Input.IsActionJustPressed("ui_left")) {
            if(!Level.StateTracker["DidMove"]) {
                GetNode<Timer>("MoveTimer").Start();
                Level.StateTracker["DidMove"] = true;
            }     
        }

        base._Process(delta);
    }
}
