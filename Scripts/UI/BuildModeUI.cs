using Godot;
namespace PlatformerWithNoJump;

public partial class BuildModeUI : Control
{
    [Export]
    public BuildModeComponent BuildModeComponent { get; set; }

    [Signal]
    public delegate void ToolBuiltEventHandler(Node2D tool, Vector2 globalPosition);

    [Export]
    public ToolSelector ToolSelector { get; set; }

    [Export]
    public DeployedToolsComponent DeployedToolsComponent { get; set; }

    private StateTracker states;

    public override void _Ready()
    {
        states = GetNode<StateTracker>("/root/StateTracker");
        BuildModeComponent.ToolBuilt += OnToolBuilt;
        base._Ready();
    }

    private void OnToolBuilt(object sender, ToolBuiltEventArgs e)
    {
        DeployedToolsComponent.Add(e.ToolBuilt);
        EmitSignal(SignalName.ToolBuilt, e.ToolBuilt, e.GlobalPos);
    }

    public override void _Process(double delta)
    {
        if(Input.IsActionJustPressed("build_mode") && !states.States["IsBuildMode"]) {
            // if(states.States["ToolSelectorOpen"] && !states.States["IsBuildMode"]) {
            BuildModeComponent.StartBuild(ToolSelector.CurrentTool);
            // }
        } else if(Input.IsActionJustPressed("build_mode") && states.States["IsBuildMode"]) {
            states.States["IsBuildMode"] = false;
        }

        base._Process(delta);
    }
}