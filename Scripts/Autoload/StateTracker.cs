using Godot;
using Godot.Collections;
namespace PlatformerWithNoJump;

public partial class StateTracker : Node
{
    private Dictionary<string, bool> states;

    public Array<Tools> UnlockedTools { get; private set; }
    
    [Signal]
    public delegate void StateChangedEventHandler(string state, bool value);

    public override void _Ready()
    {
        UnlockedTools = new() {
            Tools.Spring,
            Tools.AFP
        };

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
        EmitSignal(SignalName.StateChanged, state, value);
    }

    public bool StateExists(string state) {
        return states.ContainsKey(state);
    }
}
