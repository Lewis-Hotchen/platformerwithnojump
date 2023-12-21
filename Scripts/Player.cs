using Godot;

public partial class Player : CharacterBody2D
{
    [Export]
    private const float Gravity = 200.0f;

    [Export]
    private float GravityAccel = 1.4f;

    [Export]
    private float GravityAccelMin =  1.4f;

    [Export]
    private float terminalVelocity = 7f;

    [Export]
    private const float maxJumpVelocity = 30f;

    [Export]
    private const float minJumpVelocity=5f;

    [Export]
    private const int WalkSpeed = 80;

    private float jumpForce = 0;

    [Export]
    private float jumpAcceleration = 5f;

    private bool didJump = false;

    public override void _Ready()
    {
        base._Ready();
    }

    public override void _PhysicsProcess(double delta)
    {
        var velocity = Velocity;
        

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

        if(IsOnFloor() && didJump) {
            velocity.Y = -jumpForce;
        }

        if(!IsOnFloor()) {
            velocity.Y += (float)delta * Gravity * GravityAccel;
            GravityAccel = Mathf.Min(GravityAccel+1.4f, terminalVelocity);
        } else {
            GravityAccel = GravityAccelMin;
        }

        Velocity = velocity;
        didJump = false;
        // "MoveAndSlide" already takes delta time into account.
        MoveAndSlide();
    }

    public void Jump(float force)
    {
        jumpForce = force;
        didJump = true;
    }
}