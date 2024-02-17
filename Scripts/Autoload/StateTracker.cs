using System.Linq;
using Godot;
using Godot.Collections;
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

    public Array<Tools> UnlockedTools { get; private set; }
    
    public Dictionary<Tools, int> Resources { get; set; }

    public void UpdateResource(Tools resource, int newVal) {
        if(newVal < 0) {
            eventBus.RaiseEvent(nameof(EventBus.ToolFailed), this, new ToolFailedEventArgs(resource, FailedToolReason.RESOURCE_EMPTY));
        } else {
            Resources[resource] = newVal;
            eventBus.RaiseEvent(nameof(EventBus.StateChanged), this, new StateChangedEventArgs(nameof(Resources), resource.ToString()));
        }
    }

    public override void _Ready()
    {
        eventBus = GetNode<EventBus>("/root/EventBus");
        UnlockedTools = new() {
            Tools.Spring,
            Tools.AFP
        };

        Resources = new();

        foreach(var tool in UnlockedTools) {
            Resources[tool] = 0;
        }

        states = new Dictionary<string, bool> {
            {"IsBuildMode", false },
            {"FirstTimeBuild", false},
            {"DidMove", false},
            {"HasFallen", false},
            {"BuildEnabled", false}
        };

        base._Ready();
    }

    public bool GetState(string state) {
        return states[state];
    }

    public void SetState(string state, bool value) {
        states[state] = value;
        eventBus.RaiseEvent(nameof(EventBus.StateChanged), this, new StateChangedEventArgs(state, value));
    }

    public bool StateExists(string state) {
        return states.ContainsKey(state);
    }
}
