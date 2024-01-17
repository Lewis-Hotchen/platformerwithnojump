using System.Collections.Generic;
using Godot;
namespace PlatformerWithNoJump;

public partial class StateTracker : Node
{
    public Dictionary<string,bool> States { get; set; }

    public override void _Ready()
    {
        States = new Dictionary<string, bool> {
            {"IsBuildMode", false },
            {"ToolSelectorOpen", false},
            {"FirstTimeBuild", false},
            {"DidMove", false},
            {"HasFallen", false},
        };

        base._Ready();
    }
}
