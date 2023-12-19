using Godot;
namespace PlatformerWithNoJump;

public partial class TutorialPart1 : BaseLevel
{
    bool didMove = false;

    public override void _Ready()
    {
        GetNode<Timer>("MoveTimer").Timeout += OnMoveTimeout;
        base._Ready();
    }

    private void OnMoveTimeout()
    {
        GetNode<InGameLabel>("Player/InGameLabel").QueueFree();
    }


    public override void _Process(double delta)
    {
        if(Input.IsActionJustPressed("ui_right") || Input.IsActionJustPressed("ui_left")) {
            if(!didMove) {
                GetNode<Timer>("MoveTimer").Start();
                didMove = true;
            }     
        }

        base._Process(delta);
    }
}
