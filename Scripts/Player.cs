using System;
using Godot;
namespace PlatformerWithNoJump;

public partial class Player : CharacterBody2D, IDisposable
{
    [Export]
    public AudioStreamPlayer2D ChumRun { get; set; }

    [Export]
    public AudioStreamPlayer2D ChumHurt { get; set; }

    [Export]
    public AnimatedSprite2D ChumSprite { get; set; }

    [Export]
    public TimerTrackerComponent Timers { get; set; }

    [ExportCategory("Movement")]
    [Export]
    public float JumpHeight { get; set; }

    [Export]
    public float JumpTimeToPeak { get; set; }

    [Export]
    public float JumpTimeToDescent { get; set; }

    [Export]
    public float MoveSpeed { get; set; }
    
    [Export]
    public float Friction { get; set; }

    [Export]
    public bool CanJump { get; set; }

    private StateTracker stateTracker;
    private Impulse jumpImpulse;
    private bool pickUp = false;
    private bool pickedUp = false;
    private EventBus eventBus;
    private bool isAffectedByForce;
    private PlayerMoveStates playerState;

    private enum PlayerMoveStates
    {
        MOVING,
        IDLE,
        JUMPING,
        FALLING,
    }

    private float GetGravity(Impulse impulse)
    {
        return Velocity.Y < 0 ? impulse.JumpGravity*impulse.Direction.Y : impulse.FallGravity*impulse.Direction.Y;
    }

    public override void _Ready()
    {
        jumpImpulse = new Impulse(
            Vector2.Up, JumpTimeToDescent, JumpTimeToPeak, JumpHeight
        );

        Timers.AddTimer(0.2f, "lockout");

#if DEBUG
        pickUp = true;
#endif

        ChumSprite.FrameChanged += OnFrameChanged;

        stateTracker = GetNode<StateTracker>("/root/StateTracker");
        eventBus = GetNode<EventBus>("/root/EventBus");
        eventBus.ToolFailed += OnToolFailed;
        base._Ready();
    }

    private void OnToolFailed(object sender, ToolFailedEventArgs e)
    {
        ChumSprite.Play("chum_hurt");
        ChumHurt.Play();
    }

    private void OnFrameChanged()
    {
        if (ChumSprite.Animation == "chum_run" && IsOnFloor())
        {
            if (ChumSprite.Frame == 0 || ChumSprite.Frame == 2)
            {
                ChumRun.Play();
            }
        }
    }

    public override void _Input(InputEvent @event)
    {
        if (@event is InputEventMouseButton me)
        {
            if (me.ButtonIndex == MouseButton.Left && me.IsPressed())
            {
                pickedUp = true;
            }
            else if (me.ButtonIndex == MouseButton.Left && !me.IsPressed())
            {
                pickedUp = false;
            }
        }

        base._Input(@event);
    }

    public override void _PhysicsProcess(double delta)
    {
        playerState = PlayerMoveStates.IDLE;

        bool lockout = Timers.GetTimerRunning("lockout");
        Velocity = new(Velocity.X, Velocity.Y + GetGravity(jumpImpulse) * (float)delta);

        if (Input.IsActionJustPressed("jump") && IsOnFloor() && CanJump)
        {
            Velocity = new(Velocity.X, jumpImpulse.JumpVelocity*jumpImpulse.Direction.Y);
        }

        if(!lockout && !stateTracker.GetState(StateTracker.IsBuildMode)) {
            float dir = Input.GetAxis("left", "right");
            if(dir != 0) {
                ChumSprite.FlipH = dir > 0;
                var speed = MoveSpeed * 32;
                Velocity = new(Velocity.X + (dir * speed), Velocity.Y);
                playerState = PlayerMoveStates.MOVING;
            }
        }

        if(IsOnFloor())
        {
            Velocity = new(ApplyFriction(lockout, 0.1f), Velocity.Y);
        }
        else {
            Velocity = new (ApplyFriction(lockout, 0.1f), Velocity.Y);
            playerState = PlayerMoveStates.JUMPING;
        }

        MoveAndSlide();
        base._PhysicsProcess(delta);
    }

    private float ApplyFriction(bool lockout, float fritcion)
    {
        return Mathf.Lerp(Velocity.X, 0f, lockout ? 0f : fritcion);
    }

    public override void _Process(double delta)
    {
        if (playerState == PlayerMoveStates.MOVING)
        {
            ChumSprite.Animation = "chum_run";
        }
        else if (playerState == PlayerMoveStates.IDLE)
        {
            ChumSprite.Animation = "chum_idle";
        }

        if (playerState == PlayerMoveStates.JUMPING)
        {
            ChumSprite.Animation = "chum_fall";
        }

        if (pickedUp)
        {
            Position = GetViewport().GetMousePosition();
        }

        if (!ChumSprite.IsPlaying())
        {
            ChumSprite.Play();
        }

        base._Process(delta);
    }

    public void Impulse(
        Vector2 direction,
        float force
    )
    {
        bool leftOrRight = direction == Vector2.Right || direction == Vector2.Left;
        Timers.GetTimer("lockout").WaitTime = 0.2f;
        if(direction == Vector2.Up || direction == Vector2.Down) {
            Velocity = new(Velocity.X, 0);
        }

        if(leftOrRight) {
            Velocity = new(0, Velocity.Y);
            Timers.StartTimer("lockout");
            Timers.GetTimer("lockout").WaitTime = 0.4f;
        }

        Velocity += direction * (leftOrRight ? force * 0.8f : force);
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            eventBus.ToolFailed -= OnToolFailed;
        }

        base.Dispose(disposing);
    }
}