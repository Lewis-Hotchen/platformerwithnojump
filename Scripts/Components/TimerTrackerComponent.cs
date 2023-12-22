using System.Linq;
using Godot;

public partial class TimerTrackerComponent : Node2D
{
	[Export]
	public Timer[] Timers { get ;set; }

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public bool GetTimerRunning(string name) {
		return !GetNode<Timer>(name).IsStopped();
	}

	public Timer GetTimer(string name) {
		return GetNode<Timer>(name);
	}

	public void StartTimer(string name) {
		GetNode<Timer>(name).Start();
	}

	public Timer AddTimer(float waitTIme, string name, bool isOneShot = true) {
		var timer = new Timer() {
			WaitTime = waitTIme,
			Name = name,
			OneShot = isOneShot
		};

		Timers = Timers.Concat(new Timer[]{timer}).ToArray();

		AddChild(timer);
		return timer;
	}
}