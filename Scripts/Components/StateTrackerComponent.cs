using System.Collections.Generic;
using Godot;

public partial class StateTrackerComponent : Node2D
{
	public Dictionary<string, bool> States { get; private set; }

	[Export]
	public string[] StateNames { get; set; }

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		States = new();
		foreach(var stateName in StateNames) {
			States.Add(stateName, false);
		}
	}
}
