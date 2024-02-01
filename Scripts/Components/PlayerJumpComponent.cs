using Godot;
namespace PlatformerWithNoJump;

public partial class PlayerJumpComponent : Node2D
{
    [Export]
    public Player Player { get; set; }

    [Export]
    public float Force { get; set; }

    public override void _Process(double delta)
    {
        if (Input.IsActionJustPressed("jump"))
        {
            if (Player.IsOnFloor)
            {
                Player.GetNode<AnimatedSprite2D>("ChumSprite").Play("chum_hurt");
            }
        }

        base._Process(delta);
    }
}
