using System;
using System.Runtime.Serialization;
using Godot;
namespace PlatformerWithNoJump;

public partial class TutorialPart2 : Node2D
{
    [Export]
    public DialogueManagerComponent Dialogue;

    private StateTracker states;

    [Export]
    public Timer DialogueCooldown { get; set; }

    [Export]
    public Marker2D SpringLocOne;

    [Export]
    public Marker2D SpringLocTwo;

    bool available = false;

    public override void _Ready()
    {
        Dialogue.DialogueComplete += OnDialogueComplete;
        states = GetNode<StateTracker>("/root/StateTracker");
        available = true;
        Dialogue.SetDialogueOnBox();
        GetNode<Area2D>("Jump1Trigger").BodyEntered += OnTrigger1;
        GetNode<Area2D>("Jump2Trigger").BodyEntered += OnTrigger2;
        base._Ready();
    }

    private void OnTrigger2(Node2D body)
    {
        Dialogue.SetNextDialogueStep();
        Dialogue.SetDialogueOnBox();
    }


    private void OnTrigger1(Node2D body)
    {
        Dialogue.SetNextDialogueStep();
        Dialogue.SetDialogueOnBox();
    }


    private void OnDialogueComplete(object sender, DialogueCompleteArgs e)
    {
        if (e.CompletedStep == "Take this.")
        {
            var springOne = SceneManager.LoadScene<Node2D>("res://Scenes/Tools/Spring.tscn");
            var springTwo = SceneManager.LoadScene<Node2D>("res://Scenes/Tools/Spring.tscn");

            AddChild(springOne);
            AddChild(springTwo);

            springOne.Position = SpringLocOne.Position.Snapped(new Vector2(Constants.CellSize, Constants.CellSize));
            springOne.GetNode<ToolComponent>("ToolComponent").CanFall = true;
            springTwo.GetNode<ToolComponent>("ToolComponent").CanFall = true;
            springTwo.Position = SpringLocTwo.Position.Snapped(new Vector2(Constants.CellSize, Constants.CellSize));
            Dialogue.CanSkip = false;
            Dialogue.CanTimeOut = false;
        }
    }

    public override void _Process(double delta)
    {
        base._Process(delta);
    }
}
