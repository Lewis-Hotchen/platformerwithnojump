using System.Linq;
using Godot;
using Godot.Collections;
namespace PlatformerWithNoJump;

public partial class AFP : Node2D
{

    [Export]
    public BodyImpulseComponent ImpulseComponent { get; set; }

    [Export]
    public ToolComponent Tool { get; set; }

    [Export]
    public TimerTrackerComponent Timers { get; set; }

    [Export]

    public Area2D Area { get; set; }

    public Array<RayCast2D> GroundCasts { get; set; }

    public bool IsOnFloor => GroundCasts.Any(x => x.IsColliding());

    public override void _Ready()
    {
        GroundCasts = new()
        {
            GetNode<RayCast2D>("IsOnGround1"),
            GetNode<RayCast2D>("IsOnGround2"),
            GetNode<RayCast2D>("IsOnGround3"),
        };

        Timers.AddTimer(1, "cooldown");
        Area.BodyEntered += OnBodyEntered;
        base._Ready();
    }

    public override void _Process(double delta)
    {
        GetNode<CollisionShape2D>("CollisionShape2D").Disabled = !Tool.IsActive;
        GetNode<Area2D>("Area2D").Monitoring = Tool.IsActive;

        if (!IsOnFloor && Tool.CanFall)
        {
            Position += Vector2.Down * 500 * (float)delta;
        }

        Tool.IsActive = IsOnFloor;

        base._Process(delta);
    }

    public void SetDirection(float degrees)
    {
        switch (degrees)
        {
            case 0:
                ImpulseComponent.Direction = new Vector2(1, -1);
                break;
            case 90:
                ImpulseComponent.Direction = new Vector2(1, 1);
                break;
            case 180:
                ImpulseComponent.Direction = new Vector2(-1, 1);
                break;
            case 270:
                ImpulseComponent.Direction = new Vector2(1, -1);
                break;
        }

    }

    private void OnBodyEntered(Node2D body)
    {
        if (!Timers.GetTimerRunning("cooldown") && Tool.IsActive)
        {
            if (body is Player player)
            {
                ImpulseComponent.Apply(player);
                GetNode<AnimationPlayer>("AnimationPlayer").Play("Extend");
                Timers.StartTimer("cooldown");
            }
        }
    }
}