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

    private Vector2 jumpForce = Vector2.Zero;

    [Export]
    private float jumpAcceleration = 5f;

    private bool didApplyForce = false;

    [Export]
    public TimerTrackerComponent Timers { get; set; }

    public override void _Ready()
    {
        Timers.AddTimer(0.2f, "ForceDuration", true).Timeout += () => didApplyForce = false;
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

        if(!IsOnFloor()) {
            velocity.Y += (float)delta * Gravity * GravityAccel;
            GravityAccel = Mathf.Min(GravityAccel+1.4f, terminalVelocity);
        } else {
            GravityAccel = GravityAccelMin;
        }

        if(didApplyForce) {
            velocity = jumpForce;
        }

        // if(IsOnFloor()) {
        //     didApplyForce = false;
        // }

        Velocity = velocity;
        // "MoveAndSlide" already takes delta time into account.
        MoveAndSlide();
    }

    public void ApplyForce(Vector2 force)
    {
        jumpForce = force;
        didApplyForce = true;
        Timers.StartTimer("ForceDuration");
    }
}