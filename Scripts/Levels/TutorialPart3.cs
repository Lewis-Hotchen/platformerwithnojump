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

    private StateTracker states;

    public override void _Ready()
    {
        states = GetNode<StateTracker>("/root/StateTracker");
        var killboxDelayTimer = Timers.AddTimer(2, "KillboxDelay");
        var splashScreenTimer = Timers.AddTimer(4, "SplashScreenTimeout");
        PlayerKillBox.OnPlayerFell += PlayerFell;
        killboxDelayTimer.Timeout += FirstTimeTimeout;
        splashScreenTimer.Timeout += SplashScreenTimeout;
        base._Ready();
    }

    private void SplashScreenTimeout()
    {
        GetNode<Node2D>("Splash").QueueFree();
        states.States["HasCompletedTutorial"] = true;
        states.States["HasFallen"] = false;
        ResetLevel.ResetLevel();
    }

    private void FirstTimeTimeout()
    {
        AddChild(SceneManager.LoadScene<Node2D>("res://Scenes/UI/Splash.tscn"));
        Timers.StartTimer("SplashScreenTimeout");
    }

    private void PlayerFell(object sender, EventArgs e)
    {
        if (!states.States["HasCompletedTutorial"])
        {
            Timers.StartTimer("KillboxDelay");
        }
        else
        {
            ResetLevel.ResetLevel();
            states.States["HasFallen"] = false;
        }
    }
}
