using System;
using System.Collections.Generic;
using System.Linq;
using Godot;
namespace PlatformerWithNoJump;

public partial class Player : RigidBody2D
{
    [Export]
    public float Force { get; set; } = 3000f;

    [Export]
    public float InAirMovementReduction { get; set; } = 4f;

    [Export]
    public float GravityFloorReduction { get; set; } = 1.2f;

    [Export]
    public float DefaultGravity { get; set; } = 2f;

    [Export]
    public float MaxVelocity { get; set; } = 200f;

    [Export]
    public AudioStreamPlayer2D ChumRun { get; set; }

    [Export]
    public AudioStreamPlayer2D ChumHurt { get; set; }

    

    [Export]
    public AnimatedSprite2D ChumSprite { get; set; }

    public List<RayCast2D> GroundCasts { get; set; }

    public bool IsOnFloor => GroundCasts.Any(x => x.IsColliding());

    private StateTracker stateTracker;

    private bool firstPass;
    private bool isMoving;
    private bool pickUp = false;
    private bool pickedUp = false;
    private bool left;
    private EventBus eventBus;

    public override void _Ready()
    {
#if DEBUG
        pickUp = true;
#endif

        ChumSprite.FrameChanged += OnFrameChanged;

        ChumSprite.AnimationFinished += AnimationFinished;
        GroundCasts = new()
        {
            GetNode<RayCast2D>("IsOnGround"),
            GetNode<RayCast2D>("IsOnGround2"),
            GetNode<RayCast2D>("IsOnGround3"),
        };

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
        if (ChumSprite.Animation == "chum_run" && IsOnFloor)
        {
            if (ChumSprite.Frame == 0 || ChumSprite.Frame == 2)
            {
                ChumRun.Play();
            }
        }
    }

    private void AnimationFinished()
    {
        firstPass = true;
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

    public override void _IntegrateForces(PhysicsDirectBodyState2D state)
    {
        isMoving = false;
        //If we are in IsBuildMode we don't want the player to be able to move.
        if (stateTracker.GetState("IsBuildMode"))
        {
            return;
        }

        var direction = Vector2.Zero;
        var offset = Vector2.Zero;
        var forceToApply = Force;

        GravityScale = IsOnFloor ? GravityFloorReduction : DefaultGravity;

        if (!IsOnFloor)
        {
            forceToApply *= InAirMovementReduction;
        }

        if (Input.IsActionPressed("left"))
        {
            direction = Vector2.Left;
            left = true;
            offset = new Vector2(0, 6);
            isMoving = true;
            firstPass = true;
        }
        else if (Input.IsActionPressed("right"))
        {
            direction = Vector2.Right;
            left = false;
            offset = new Vector2(12, 6);
            isMoving = true;
            firstPass = true;
        }

        if (LinearVelocity.X >= MaxVelocity || LinearVelocity.X <= -MaxVelocity)
        {
            return;
        }

        state.ApplyForce(direction * forceToApply, offset);
        base._IntegrateForces(state);
    }

    public override void _Process(double delta)
    {
        if (!isMoving && firstPass)
        {
            ChumSprite.Animation = "chum_idle";
            ChumSprite.Play();
            firstPass = false;
        }

        if (isMoving)
        {
            ChumSprite.Animation = "chum_run";
            ChumSprite.Play();
            ChumSprite.FlipH = !left;
        }

        if (pickedUp)
        {
            Position = GetViewport().GetMousePosition();
        }

        base._Process(delta);
    }

    public void Jump(float force)
    {
        if (IsOnFloor)
        {
            ApplyImpulse(Vector2.Up * force);
        }
    }
}