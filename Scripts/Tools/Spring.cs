using System.Linq;
using System.Windows.Markup;
using Godot;
using Godot.Collections;
namespace PlatformerWithNoJump;

public partial class Spring : StaticBody2D
{
    [Export]
    public TimerTrackerComponent TimeTracker { get; set; }

    [Export]
    public BodyImpulseComponent BodyImpulseComponent { get; set; }

    [Export]
    public Vector2 Direction { get; set; }

    [Export]
    public ToolComponent Tool { get; set; }

    [Export]
    public AnimatedSprite2D Sprite { get; set; }

    public Tools ToolType = Tools.Spring;

    public Array<RayCast2D> GroundCasts { get; set; }

    public bool IsOnFloor => GroundCasts.Any(x => x.IsColliding());

    private bool isPlaced;

    public override void _Ready()
    {
        GroundCasts = new()
        {
            GetNode<RayCast2D>("IsOnGround1"),
            GetNode<RayCast2D>("IsOnGround2"),
            GetNode<RayCast2D>("IsOnGround3"),
        };

        BodyImpulseComponent.Direction = Direction;

        TimeTracker.AddTimer(1, "cooldown");

        TimeTracker.Subscribe("cooldown", () =>
        {
            Sprite.PlayBackwards();
        });

        GetNode<Area2D>("Area2D").BodyEntered += OnBodyEntered;
        base._Ready();
    }

    public override void _Process(double delta)
    {
        GetNode<CollisionShape2D>("CollisionShape2D").Disabled = !Tool.IsActive;
        GetNode<Area2D>("Area2D").Monitoring = Tool.IsActive;

        Tool.IsActive = IsOnFloor;

        if (!IsOnFloor && Tool.CanFall)
        {
            Position += Vector2.Down * 500 * (float)delta;
        }

        base._Process(delta);
    }

    public void SetDirection(float degrees)
    {
        switch (degrees)
        {
            case 0:
                BodyImpulseComponent.Direction = Vector2.Up;
                break;
            case 90:
                BodyImpulseComponent.Direction = Vector2.Right;
                break;
            case 180:
                BodyImpulseComponent.Direction = Vector2.Down;
                break;
            case 270:
                BodyImpulseComponent.Direction = Vector2.Left;
                break;
        }
    }

    private void OnBodyEntered(Node2D body)
    {
        if (Tool.IsActive)
        {
            if (body is Player player)
            {
                BodyImpulseComponent.Apply(player, true);
                GetNode<AnimationPlayer>("AnimationPlayer").Play("Extend");
                Sprite.Play();
                TimeTracker.StartTimer("cooldown");
            }
        }
    }
}