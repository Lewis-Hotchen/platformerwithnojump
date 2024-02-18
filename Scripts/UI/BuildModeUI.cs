using System;
using Godot;
namespace PlatformerWithNoJump;

public partial class BuildModeUI : Control
{
    [Export]
    public BuildModeComponent BuildModeComponent { get; set; }

    [Export]
    public AudioStreamPlayer2D BuildModeEnabled { get; set; }

    [Export]
    public AudioStreamPlayer2D BuildModeDisabled { get; set; }

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

    private void OnStateChanged(object sender, StateChangedEventArgs e)
    {
        switch (e.State)
        {
            case "BuildEnabled":
                Visible = e.Value.AsBool();
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
        DeployedToolsComponent.Add(e.Tool);
        eventBus.RaiseEvent(
            nameof(EventBus.ToolBuilt),
            this,
            new ToolBuiltEventArgs(e.Tool, e.GlobalPosition));
        states.UpdateResource(ToolSelector.CurrentToolType, states.Resources[ToolSelector.CurrentToolType] - 1);
    }

    public override void _Process(double delta)
    {
        if (Input.IsActionJustPressed("build_mode") && !states.GetState("IsBuildMode"))
        {
            if (states.Resources[ToolSelector.CurrentToolType] > 0)
            {
                BuildModeComponent.StartBuild(ToolSelector.CurrentTool);
                states.SetState("IsBuildMode", true);
                BuildModeEnabled.Play();
            }
            else
            {
                eventBus.RaiseEvent(nameof(EventBus.ToolFailed), this, new ToolFailedEventArgs(ToolSelector.CurrentToolType, FailedToolReason.RESOURCE_EMPTY));
            }
        }
        else if (Input.IsActionJustPressed("build_mode") && states.GetState("IsBuildMode"))
        {
            states.SetState("IsBuildMode", false);
            BuildModeDisabled.Play();
        }

        if (Input.IsActionJustPressed("revert"))
        {
            var refundedType = DeployedToolsComponent.RemoveLast();
            states.UpdateResource(refundedType, states.Resources[refundedType] + 1);
        }

        base._Process(delta);
    }
}