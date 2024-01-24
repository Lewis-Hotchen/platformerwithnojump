using Godot;

namespace PlatformerWithNoJump;

public partial class BodyImpulseComponent : Node2D
{
    [Export]
    public float Force { get; set; }

    [Export]
    public Vector2 Direction { get; set; }

    public void Apply(RigidBody2D actor)
    {
        actor.LinearVelocity = Vector2.Zero;
        actor.ApplyImpulse(Direction * Force);
    }
}
