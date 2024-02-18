using Godot;
using PlatformerWithNoJump;

public partial class L11 : Node2D
{
    private StateTracker states;

    [Export]
    public DialogueManagerComponent DialogueManagerComponent { get; set; }

    public override void _Ready()
    {
        DialogueManagerComponent.SetDialogueOnBox();
        states = GetNode<StateTracker>("/root/StateTracker");
        states.UpdateResource(Tools.Spring, 4);
        base._Ready();
    }
}