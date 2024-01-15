using Godot;
using PlatformerWithNoJump;

public partial class BuildModeUI : Control
{
    [Export]
    public BuildModeComponent BuildModeComponent { get; set; }

    [Signal]
    public delegate void ToolBuiltEventHandler(Node2D tool);

    [Export]
    public ToolSelector ToolSelector { get; set; }

    [Export]
    public StateTrackerComponent StateTrackerComponent { get; set;}

    [Export]
    public DeployedToolsComponent DeployedToolsComponent { get; set; }

    public override void _Ready()
    {
        BuildModeComponent.StateTrackerComponent = StateTrackerComponent;
        ToolSelector.States = StateTrackerComponent;
        StateTrackerComponent.States["ToolSelectorOpen"] = ToolSelector.Visible;
        BuildModeComponent.ToolBuilt += OnToolBuilt;
        ToolSelector.ToolSelected += OnToolSelected;
        base._Ready();
    }

    private void OnToolBuilt(object sender, ToolBuiltEventArgs e)
    {
        DeployedToolsComponent.Add(e.ToolBuilt);
        e.ToolBuilt.GlobalPosition = e.GlobalPos;
        EmitSignal(SignalName.ToolBuilt, e.ToolBuilt);
    }


    private void OnToolSelected(object sender, ToolSelectedEventArgs e)
    {
    }


    public override void _Process(double delta)
    {
        if(Input.IsActionJustPressed("ui_up")) {
            StateTrackerComponent.States["ToolSelectorOpen"] = !StateTrackerComponent.States["ToolSelectorOpen"];
            ToolSelector.Visible = StateTrackerComponent.States["ToolSelectorOpen"];
        }

        base._Process(delta);
    }
}
