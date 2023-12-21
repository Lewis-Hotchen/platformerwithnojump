using Godot;
using PlatformerWithNoJump;

public partial class TutorialPart2 : Node2D
{
    [Export]
    public BaseLevel Level { get; set; }

    public override void _Process(double delta)
    {
        if(Input.IsActionJustPressed("ui_accept")) {
            Level.Player.Jump(200f);
        }

        base._Process(delta);
    }
}
