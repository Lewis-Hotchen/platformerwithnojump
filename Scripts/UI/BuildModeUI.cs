using System;
using System.Linq;
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
        HandleRevertInput(delta);
        HandleLastResort();

        base._Process(delta);
    }

    private void HandleLastResort()
    {
        if(Input.IsActionJustPressed("jump") && states.GetState(StateTracker.IsBuildMode)) {
            if(states.UnlockedTools.Contains(Tools.Leg)) {
                return;
            } else {
                states.UnlockedTools.Add(Tools.Leg);
                states.Resources.Add(Tools.Leg, new ToolResource() {
                    Max = 2,
                    Current = 0
                });

                states.UpdateResource(Tools.Leg, 2);
                states.SetState(StateTracker.IsLastResortActive, true);
            }
        }
    }

    private void HandleRevertInput(double delta)
    {
        if (Input.IsActionPressed("revert") && (DeployedToolsComponent.DeployedTools.Any() || states.GetState(StateTracker.IsLastResortActive)))
        {
            isHoldingRevert = true;
            if(heldTime >= 2) {
                foreach(var tool in DeployedToolsComponent.DeployedTools) {
                    var animationPlayer = tool.GetNode<AnimationPlayer>("AnimationPlayer");
                    if(!animationPlayer.IsPlaying()) {
                        animationPlayer.Play("Revert");
                        tool.GetNode<ToolComponent>("ToolComponent").IsActive = false;
                    }
                }
            }
        }
        else
        {
            {
                if (heldTime >= 2 && isHoldingRevert)
                {
                    RevertAll();
                    eventBus.RaiseEvent("RevertAll", this, new EventArgs());
                }

                foreach(var tool in DeployedToolsComponent.DeployedTools) {
                    tool.GetNode<ToolComponent>("ToolComponent").IsActive = true;
                }

                heldTime = 0;
                isHoldingRevert = false;
            }
        }

        if (isHoldingRevert)
        {
            heldTime += delta;
        }
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