using System;
using Godot;
namespace PlatformerWithNoJump;

public partial class BuildModeUI : Control, IDisposable
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

    [Export]
    public TimerTrackerComponent Timers { get; set; }

    private StateTracker states;
    private EventBus eventBus;
    private bool isHoldingRevert;
    private double heldTime;

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
        states.UpdateResource(ToolSelector.CurrentToolType, states.Resources[ToolSelector.CurrentToolType].Current - 1);
    }

    public override void _Input(InputEvent @event)
    {
        if (@event.IsActionPressed("revert"))
        {
            isHoldingRevert = true;
        } else {

            if(isHoldingRevert) {
                if(heldTime < 2) {
                    Revert();
                } else {
                    RevertAll();
                }

                heldTime = 0;
                isHoldingRevert = false;
            }
        }

        base._Input(@event);
    }

    private void RevertAll()
    {
        DeployedToolsComponent.Reset();
        states.UpdateResource(Tools.Spring, states.Resources[Tools.Spring].Max);
    }

    private void Revert()
    {
            var refundedType = DeployedToolsComponent.RemoveLast();
            if (refundedType == Tools.None)
            {
                refundedType = ToolSelector.CurrentToolType;
            }

            states.UpdateResource(refundedType, states.Resources[refundedType].Current + 1);
    }


    public override void _Process(double delta)
    {

        HandleBuildModeInput();

        if(isHoldingRevert) {
            heldTime += delta;
        }

        base._Process(delta);
    }

    private void HandleBuildModeInput()
    {
        if (!states.GetState(StateTracker.BuildEnabled))
        {
            return;
        }

        if (Input.IsActionJustPressed("cancel") && states.GetState("IsBuildMode"))
        {
            states.SetState("IsBuildMode", false);
        }

        if (Input.IsActionJustPressed("build_mode"))
        {
            if (!states.GetState("IsBuildMode"))
            {
                BuildModeComponent.StartBuild(ToolSelector.CurrentTool);
                states.SetState("IsBuildMode", true);
                BuildModeEnabled.Play();
            }
            else
            {
                bool success = BuildModeComponent.FinishBuild();
                if (success)
                {
                    states.SetState("IsBuildMode", false);
                    BuildModeDisabled.Play();
                }
                else
                {
                    eventBus.RaiseEvent(nameof(EventBus.ToolFailed), this, new ToolFailedEventArgs(ToolSelector.CurrentToolType, FailedToolReason.RESOURCE_EMPTY));
                }
            }
        }
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            eventBus.StateChanged -= OnStateChanged;
            eventBus.ToolBuilt -= OnToolBuilt;
        }

        base.Dispose(disposing);
    }
}