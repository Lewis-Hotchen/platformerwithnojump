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

    public void Apply(RigidBody2D actor, bool playSound = false)
    {
        bool leftOrRight = Direction == Vector2.Right || Direction == Vector2.Left;
        actor.LinearVelocity = new Vector2(actor.LinearVelocity.X, 0);
        actor.ApplyImpulse(Direction * (leftOrRight ? Force * 1.5f : Force));

        if (playSound) ChumJump.Play();
    }
}
