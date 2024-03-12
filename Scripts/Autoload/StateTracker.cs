using System;
using System.Collections.Generic;
using System.Linq;
using Godot;
namespace PlatformerWithNoJump;

public partial class StateTracker : Node
{
    private Dictionary<string, bool> states;
    private EventBus eventBus;

    public readonly static string IsBuildMode = "IsBuildMode";
    public readonly static string FirstTimeBuild = "FirstTimeBuild";
    public readonly static string DidMove = "DidMove";
    public readonly static string HasFallen = "HasFallen";
    public readonly static string BuildEnabled = "BuildEnabled";
    public readonly static string ResourcesState = "Resources";
    public readonly static string IsLastResortActive = "IsLastResortActive";
    
    public List<Tools> UnlockedTools { get; private set; }

    public Dictionary<Tools, ToolResource> Resources { get; set; }

    public void UpdateResource(Tools resource, int newVal)
    {
        if (resource == Tools.None)
        {
            return;
        }

        if (newVal < 0)
        {
            eventBus.RaiseEvent(nameof(EventBus.ToolFailed), this, new ToolFailedEventArgs(resource, FailedToolReason.RESOURCE_EMPTY));
        }
        else
        {
            if (Resources[resource].Max < newVal)
            {
                eventBus.RaiseEvent(nameof(EventBus.ToolFailed), this, new ToolFailedEventArgs(resource, FailedToolReason.NO_REVERT));
            }
            else
            {
                Resources[resource].Current = newVal;
                eventBus.RaiseEvent(nameof(EventBus.StateChanged), this, new StateChangedEventArgs(nameof(Resources), resource.ToString()));
            }

        }
    }

    public void SetupLevel(Dictionary<Tools, ToolResource> resources, bool buildEnabled, Tools[] unlockedTools)
    {

        UnlockedTools = new(unlockedTools);
        if (!resources.Keys.Any(x => UnlockedTools.Any(y => y == x)) && resources.Any())
        {
            throw new ArgumentException("Added a tool which was not unlocked");
        }

        Resources = resources;
        eventBus.RaiseEvent(nameof(EventBus.StateChanged), this, new StateChangedEventArgs(nameof(Resources), ResourcesState));
        SetState(BuildEnabled, buildEnabled);
    }

    public override void _Ready()
    {
        eventBus = GetNode<EventBus>("/root/EventBus");
        UnlockedTools = new() {
            Tools.Spring
        };

        Resources = new();

        foreach (var tool in UnlockedTools)
        {
            Resources[tool] = new ToolResource()
            {
                Max = 0,
                Current = 0
            };
        }

        states = new Dictionary<string, bool> {
            {"IsBuildMode", false },
            {"FirstTimeBuild", false},
            {"DidMove", false},
            {"HasFallen", false},
            {"BuildEnabled", false},
            {"IsLastResortActive", false}
        };

        base._Ready();
    }

    public bool GetState(string state)
    {
        return states[state];
    }

    public void SetState(string state, bool value)
    {
        states[state] = value;
        eventBus.RaiseEvent(nameof(EventBus.StateChanged), this, new StateChangedEventArgs(state, value));
    }

    public bool StateExists(string state)
    {
        return states.ContainsKey(state);
    }
}

public class ToolResource
{
    public int Max { get; set; }
    public int Current { get; set; }
}