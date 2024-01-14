using System;
using Godot;
using PlatformerWithNoJump;

public partial class BuildModeUI : Control
{
    [Export]
    public BuildModeComponent BuildModeComponent { get; set; }

    [Export]
    public ToolSelector ToolSelector { get; set; }

    [Export]
    public StateTrackerComponent StateTrackerComponent { get; set;}

    public DeployedToolsComponent DeployedToolsComponent { get; set; }

    public override void _Ready()
    {
        StateTrackerComponent.States["ToolSelectorOpen"] = ToolSelector.Visible;
        DeployedToolsComponent = GetNode<DeployedToolsComponent>("../DeployedToolsComponent");
        BuildModeComponent.ToolBuilt += OnToolBuilt;
        ToolSelector.ToolSelected += OnToolSelected;
        base._Ready();
    }

    private void OnToolBuilt(object sender, ToolBuiltEventArgs e)
    {
        DeployedToolsComponent.Add(e.ToolBuilt, e.GlobalPos);
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

        if(StateTrackerComponent.States["IsBuildMode"])
            QueueRedraw();

        base._Process(delta);
    }
}
