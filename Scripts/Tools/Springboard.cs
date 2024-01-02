using Godot;
namespace PlatformerWithNoJump;

public partial class Springboard : Node2D {

    [Export]
	public BodyImpulseComponent ImpulseComponent { get; set; }

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