using System;
using Godot;

namespace PlatformerWithNoJump;
public partial class TutorialPart3 : Node2D
{
    [Export]
    public PlayerKillBoxComponent PlayerKillBox { get; set; }

    [Export]
    public ResetLevelComponent ResetLevel { get; set; }

    [Export]
    public TimerTrackerComponent Timers { get; set; }

    [Export]
    public DialogueManagerComponent Dialogue { get; set; }

    private StateTracker states;

    public override void _Ready()
    {
        states = GetNode<StateTracker>("/root/StateTracker");
        Dialogue.SetDialogueOnBox();
        Dialogue.DialogueComplete += OnDialogueComplete;
        PlayerKillBox.OnPlayerFell += OnPlayerFell;
        base._Ready();
    }

    private void OnDialogueComplete(object sender, DialogueCompleteArgs e)
    {
        if(e.CompletedStep == "Press B to activate build mode.") {
            states.SetState("BuildEnabled", true);
        }
    }


    private void OnPlayerFell(object sender, EventArgs e)
    {
        GetNode<ScreenCamera>("../../Camera").ApplyShake();
    }


    private void SplashScreenTimeout()
    {
        GetNode<Node2D>("Splash").QueueFree();
        states.SetState("HasCompletedTutorial", true);
        states.SetState("HasFallen", false);
        ResetLevel.ResetLevel();
    }

    private void FirstTimeTimeout()
    {
        AddChild(SceneManager.LoadScene<Node2D>("res://Scenes/UI/Splash.tscn"));
        Timers.StartTimer("SplashScreenTimeout");
    }
}