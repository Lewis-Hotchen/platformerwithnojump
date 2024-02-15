using Godot;
using Godot.Collections;
namespace PlatformerWithNoJump;

public partial class StateTracker : Node
{
    private Dictionary<string, bool> states;
    private EventBus eventBus;

    public Array<Tools> UnlockedTools { get; private set; }
    
    public Dictionary<Tools, int> Resources { get; set; }

    public void UpdateResource(Tools resource, int newVal) {
        if(newVal < 0) {
            eventBus.RaiseEvent(EventBus.SignalName.ToolFailed, this);
        } else {
            Resources[resource] = newVal;
            eventBus.RaiseEvent(EventBus.SignalName.StateChanged, this, nameof(Resources), resource.ToString());
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
        eventBus.RaiseEvent(EventBus.SignalName.StateChanged, this, state, value);
    }

    public bool StateExists(string state) {
        return states.ContainsKey(state);
    }
}
