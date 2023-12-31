using System;
using Godot;

namespace PlatformerWithNoJump;
public partial class TutorialPart3 : Node2D
{
    [Export]
    public PlayerKillBoxComponent PlayerKillBox { get; set; }

    [Export]
    public StateTrackerComponent StateTracker { get; set; }

    [Export]
    public ResetLevelComponent ResetLevel {get;set;}

    [Export]
    public TimerTrackerComponent Timers {get;set;}

    public override void _Ready()
    {
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
        StateTracker.States["HasCompletedTutorial"] = true;
        StateTracker.States["HasFallen"] = false;
        ResetLevel.ResetLevel();
        var spring = SceneManager.LoadScene<Springboard>("res://Scenes/Tools/Springboard.tscn");
        spring.Position = ResetLevel.StartPosition + new Vector2(36, 6);
        spring.JumpComponent.Player = GetNode<Player>("Player");
        AddChild(spring);
    }

    private void FirstTimeTimeout()
    {
        AddChild(SceneManager.LoadScene<Node2D>("res://Scenes/Splash.tscn"));
        Timers.StartTimer("SplashScreenTimeout");
    }

    private void PlayerFell(object sender, EventArgs e)
    {
        if(!StateTracker.States["HasCompletedTutorial"]) {
            Timers.StartTimer("KillboxDelay");
        } else {
            ResetLevel.ResetLevel();
            StateTracker.States["HasFallen"] = false;
        }
    }
}
