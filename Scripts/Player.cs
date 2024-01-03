using System.Collections.Generic;
using System.Linq;
using Godot;
namespace PlatformerWithNoJump;

public partial class Player : RigidBody2D
{

    [Export]
    public float Force { get; set; } = 1500f;

    [Export]
    public ToolSelector ToolSelector { get; set; }

    [Export]
    public float InAirMovementReduction { get; set;} = 2f;

    [Export]
    public float GravityFloorReduction { get; set; } = 0.6f;

    [Export]
    public float DefaultGravity { get; set; } = 1f;

    [Export]
    public float MaxVelocity { get; set; } = 100f;

    [Export]
    public StateTrackerComponent States {get;set;}

    public List<RayCast2D> GroundCasts { get; set; }

    public bool IsOnFloor => GroundCasts.Any(x => x.IsColliding());

    public override void _Ready()
    {
        GroundCasts = new()
        {
            GetNode<RayCast2D>("IsOnGround"),
            GetNode<RayCast2D>("IsOnGround2"),
            GetNode<RayCast2D>("IsOnGround3"),
        };
        base._Ready();
    }

    public override void _IntegrateForces(PhysicsDirectBodyState2D state)
    {
        var direction = Vector2.Zero;
        var offset = Vector2.Zero;
        var forceToApply = Force;

        GravityScale = IsOnFloor ? GravityFloorReduction : DefaultGravity;

        if(!IsOnFloor) {
            forceToApply /= InAirMovementReduction;
        }

        if(Input.IsActionPressed("ui_left")) {
            direction = Vector2.Left;
            offset = new Vector2(0, 3);
        } else if(Input.IsActionPressed("ui_right")) {
             direction = Vector2.Right;
             offset = new Vector2(6, 3);
        }

        if(LinearVelocity.X >= MaxVelocity) {
            return;
        }

        state.ApplyForce(direction * forceToApply, offset);
        base._IntegrateForces(state);
    }

    public override void _Process(double delta)
    {
        ToolSelector.Visible = States.States["ToolSelectorOpen"];

        if(Input.IsActionJustPressed("ui_up")) {
            States.States["ToolSelectorOpen"] = !States.States["ToolSelectorOpen"];
        }

        base._Process(delta);
    }

    public void Jump(float force)
    {
        if(IsOnFloor) {
            ApplyImpulse(Vector2.Up*force);
        }
    }
}