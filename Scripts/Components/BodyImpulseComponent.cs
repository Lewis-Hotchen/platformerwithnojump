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
        actor.LinearVelocity = Vector2.Zero;
        actor.ApplyImpulse(Direction * Force);

        if(playSound) ChumJump.Play();
    }
}
