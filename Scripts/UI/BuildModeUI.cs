using System;
using Godot;
namespace PlatformerWithNoJump;

public partial class BuildModeUI : Control
{
    [Export]
    public BuildModeComponent BuildModeComponent { get; set; }

    [Export]
    public ToolSelector ToolSelector { get; set; }

    [Export]
    public DeployedToolsComponent DeployedToolsComponent { get; set; }

    private StateTracker states;
    private EventBus eventBus;

    public override void _Ready()
    {
        states = GetNode<StateTracker>("/root/StateTracker");
        eventBus = GetNode<EventBus>("/root/EventBus");
        eventBus.StateChanged += OnStateChanged;
        BuildModeComponent.ToolBuilt += OnToolBuilt;
        base._Ready();
    }

    private void OnStateChanged(Node sender, string state, Variant value)
    {
        switch(state) {
            case "BuildEnabled":
                Visible = value.AsBool();
                break;
        }
    }

    public override void _ExitTree()
    {
        BuildModeComponent.ToolBuilt -= OnToolBuilt;
        base._ExitTree();
    }

    private void OnToolBuilt(object sender, ToolBuiltEventArgs e)
    {
        DeployedToolsComponent.Add(e.ToolBuilt);
        eventBus.RaiseEvent(EventBus.SignalName.ToolBuilt, this, e.ToolBuilt, e.GlobalPos);
        states.UpdateResource(ToolSelector.CurrentToolType, states.Resources[ToolSelector.CurrentToolType]-1);
    }

    public override void _Process(double delta)
    {
        if (Input.IsActionJustPressed("build_mode") && !states.GetState("IsBuildMode"))
        {
            if(states.Resources[ToolSelector.CurrentToolType] > 0) {
                BuildModeComponent.StartBuild(ToolSelector.CurrentTool);
            } else {
                eventBus.RaiseEvent(EventBus.SignalName.ToolFailed, this);
            }
        }
        else if (Input.IsActionJustPressed("build_mode") && states.GetState("IsBuildMode"))
        {
            states.SetState("IsBuildMode", false);
        }

        base._Process(delta);
    }
}