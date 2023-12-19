using Godot;

public partial class Player : CharacterBody2D
{
    private const float Gravity = 200.0f;
    private const int WalkSpeed = 80;
    
    [Export]
    public int Speed { get; set; } = 1;

    public override void _PhysicsProcess(double delta)
    {
        var velocity = Velocity;

        velocity.Y += (float)delta * Gravity;

        if (Input.IsActionPressed("ui_left"))
        {
            velocity.X = -WalkSpeed;
        }
        else if (Input.IsActionPressed("ui_right"))
        {
            velocity.X = WalkSpeed;
        }
        else
        {
            velocity.X = 0;
        }

        Velocity = velocity;

        // "MoveAndSlide" already takes delta time into account.
        MoveAndSlide();
    }
}