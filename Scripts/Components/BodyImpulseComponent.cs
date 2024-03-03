using Godot;

namespace PlatformerWithNoJump;

public partial class BodyImpulseComponent : Node2D
{
    [Export]
    public float Force { get; set; }

    [Export]
    public Vector2 Direction { get; set; }

    [Export]
    public AudioStreamPlayer2D ChumJump { get; set; }

    public void Apply(Player actor, bool playSound = false)
    {
        actor.Impulse(Direction, Force);
        if (playSound) ChumJump.Play();
    }
}
