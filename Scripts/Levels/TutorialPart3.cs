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
        states.SetupLevel(new System.Collections.Generic.Dictionary<Tools, ToolResource>() {
            {
                Tools.Spring,
                new ToolResource() {
                    Current = 3,
                    Max = 3
                }
            }
        }, true, new[] { Tools.Spring });

        Dialogue.DialogueComplete += OnDialogueComplete;
        PlayerKillBox.OnPlayerFell += OnPlayerFell;

        base._Ready();
    }

    private void OnDialogueComplete(object sender, DialogueCompleteArgs e)
    {
        if (e.CompletedStep == "Press B to activate build mode.")
        {
            states.SetState("BuildEnabled", true);
        }
    }

    private void OnPlayerFell(object sender, EventArgs e)
    {
        var resetTimer = Timers.AddTimer(2, "reset", () =>
        {
            ResetLevel.ResetLevel();
            if (Dialogue.DialogueEntry != "Exemplary.")
                Dialogue.SetDialogueOnBox();
        });

        GetNode<ScreenCamera>("../../Camera").ApplyShake();
        Dialogue.AnimationPlayer.Pause();
        Timers.OneShot(3, () =>
        {
            Dialogue.OneShotDialog("We've all been there.");
            resetTimer.Start();
        });
    }

    private void SplashScreenTimeout()
    {
        GetNode<Node2D>("Splash").QueueFree();
        states.SetState("HasCompletedTutorial", true);
        states.SetState(StateTracker.HasFallen, false);
        ResetLevel.ResetLevel();
    }

    private void FirstTimeTimeout()
    {
        AddChild(SceneManager.LoadScene<Node2D>("res://Scenes/UI/Splash.tscn"));
        Timers.StartTimer("SplashScreenTimeout");
    }
}