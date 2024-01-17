using Godot;
namespace PlatformerWithNoJump;

public partial class Bouncepad : Node2D, ITool
{
    [Export]
    public TimerTrackerComponent TimeTracker { get; set; }

    [Export]
    public BodyImpulseComponent BodyImpulseComponent { get; set; }

    public Tools ToolType => Tools.Bouncepad;

    public bool IsPlaceable => true;


    public override void _Ready()
    {
        TimeTracker.AddTimer(1, "cooldown");
        GetNode<Area2D>("Area2D").BodyEntered += OnBodyEntered;
        base._Ready();
    }

    private void OnBodyEntered(Node2D body)
	{
		if (!TimeTracker.GetTimerRunning("cooldown"))
		{
			if (body is Player player)
			{
				BodyImpulseComponent.Apply(player);
				GetNode<AnimationPlayer>("AnimationPlayer").Play("Extend");
				TimeTracker.StartTimer("cooldown");
			}
		}
	}
}
