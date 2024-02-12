using Godot;
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

    public override void _Ready()
    {
        Timers.AddTimer(1, "cooldown");
        Area.BodyEntered += OnBodyEntered;
        base._Ready();
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
        if (!Timers.GetTimerRunning("cooldown"))
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