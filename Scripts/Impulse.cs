using Godot;

namespace PlatformerWithNoJump;

public class Impulse
{
    public float JumpHeight { get; }
    public float TimeToPeak { get; }
    public float TimeToDescent { get; }
    public Vector2 Direction { get; }

    public float JumpVelocity => 2 * 32 * JumpHeight / TimeToPeak;
    public float JumpGravity => -2 * 32 * JumpHeight / (TimeToPeak * TimeToPeak);
    public float FallGravity => -2 * 32 * JumpHeight / (TimeToDescent * TimeToDescent);

    public Impulse(Vector2 direction, float timeToDescent, float timeToPeak, float jumpHeight)
    {
        Direction = direction;
        TimeToDescent = timeToDescent;
        TimeToPeak = timeToPeak;
        JumpHeight = jumpHeight;
    }
}
