using Godot;
namespace PlatformerWithNoJump;

public partial class TutorialPart1 : Node2D
{
    [Export]
    public DialogueManagerComponent Dialogue;
    
    public override void _Ready()
    {
        states = GetNode<StateTracker>("/root/StateTracker");
        states.SetState("BuildEnabled", false);
        Dialogue.SetDialogueOnBox();
        GetNode<Area2D>("Area2D").BodyShapeEntered += OnBodyShapeEntered;
        base._Ready();
    }
 
    private void OnBodyShapeEntered(Rid bodyRid, Node2D body, long bodyShapeIndex, long localShapeIndex)
    {
        Dialogue.SetNextDialogueStep();
        Dialogue.SetDialogueOnBox();
    }

    private StateTracker states;

    public override void _Process(double delta)
    {
        base._Process(delta);
    }
}
