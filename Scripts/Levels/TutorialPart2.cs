using Godot;

public partial class TutorialPart2 : Node2D
{
    [Export]
    public PlayerJumpComponent PlayerJumpComponent { get; set; }

    public override void _Process(double delta)
    {
        if(Input.IsActionJustPressed("ui_accept")) {
            PlayerJumpComponent.Jump();
        }
    }
}
