using System;
using Godot;

namespace PlatformerWithNoJump;

public partial class L12 : Node2D
{
    private StateTracker states;

    [Export]
    public DialogueManagerComponent DialogueManagerComponent { get; set; }
    
    [Export]
    public ResetLevelComponent ResetLevelComponent { get; set; }

    [Export]
    public PlayerKillBoxComponent PlayerKillBoxComponent { get; set; }

    public override void _Ready()
    {
        DialogueManagerComponent.SetDialogueOnBox();
        states = GetNode<StateTracker>("/root/StateTracker");
        states.SetupLevel(
            new System.Collections.Generic.Dictionary<Tools, ToolResource> {
                {
                Tools.Spring,
                new ToolResource() {
                    Max = 1,
                    Current = 1
                }
                }
            },
            true,
            new Tools[] {
                Tools.Spring
            }
        );

        base._Ready();
    }

    public override void _Process(double delta)
    {
        
        base._Process(delta);
    }
}
