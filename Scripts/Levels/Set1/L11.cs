using Godot;
namespace PlatformerWithNoJump;

public partial class L11 : Node2D
{
    private StateTracker states;

    [Export]
    public DialogueManagerComponent DialogueManagerComponent { get; set; }

    public override void _Ready()
    {
        DialogueManagerComponent.SetDialogueOnBox();
        states = GetNode<StateTracker>("/root/StateTracker");
        states.SetupLevel(
            new System.Collections.Generic.Dictionary<Tools, ToolResource> {
                {
                Tools.Spring,
                new ToolResource() {
                    Max = 4,
                    Current = 4
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
}