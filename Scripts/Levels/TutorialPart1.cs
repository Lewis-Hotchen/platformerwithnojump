using Godot;
namespace PlatformerWithNoJump;

public partial class TutorialPart1 : Node2D
{
    [Export]
    public StateTrackerComponent StateTracker { get; set; }
    public override void _Ready()
    {
        GetNode<Timer>("MoveTimer").Timeout += OnMoveTimeout;
        GetNode<Springboard>("Springboard").JumpComponent.Player = GetNode<Player>("Player");
        base._Ready();
    }

    private void OnMoveTimeout()
    {
        GetNode<InGameLabel>("InGameLabel").QueueFree();
    }


    public override void _Process(double delta)
    {
        if(Input.IsActionJustPressed("ui_right") || Input.IsActionJustPressed("ui_left")) {
            if(!StateTracker.States["DidMove"]) {
                GetNode<Timer>("MoveTimer").Start();
                StateTracker.States["DidMove"] = true;
            }     
        }

        base._Process(delta);
    }
}
