using Godot;
namespace PlatformerWithNoJump;

public partial class BuildModeUI : Control
{
    [Export]
    public BuildModeComponent BuildModeComponent { get; set; }

    [Signal]
    public delegate void ToolBuiltEventHandler(Node2D tool);

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
        e.ToolBuilt.GlobalPosition = e.GlobalPos;
        EmitSignal(SignalName.ToolBuilt, e.ToolBuilt);
    }

    public override void _Process(double delta)
    {
        if(Input.IsActionJustPressed("build_mode")) {
            states.States["ToolSelectorOpen"] = !states.States["ToolSelectorOpen"];
            ToolSelector.Visible = states.States["ToolSelectorOpen"];
            if(states.States["ToolSelectorOpen"] && !states.States["IsBuildMode"]) {
                BuildModeComponent.StartBuild(ToolSelector.Preview, ToolSelector.SelectedToolType);
            }
        }

        base._Process(delta);
    }
}