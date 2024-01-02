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
        GetNode<Player>("Player").ToolSelector.ToolSelected += OnToolSelected;
        base._Ready();
    }

    private void OnToolSelected(object sender, ToolSelectedEventArgs e)
    {
        var springboard = SceneManager.LoadScene<Springboard>("res://Scenes/Tools/" + e.ScenePath + ".tscn");
        springboard.Position = new Vector2(
            GetNode<Player>("Player").Position.X + 24,
            GetNode<Player>("Player").Position.Y + 6);
        AddChild(springboard);
    }


    private void SplashScreenTimeout()
    {
        GetNode<Node2D>("Splash").QueueFree();
        StateTracker.States["HasCompletedTutorial"] = true;
        StateTracker.States["HasFallen"] = false;
        ResetLevel.ResetLevel();

    }

    private void FirstTimeTimeout()
    {
        AddChild(SceneManager.LoadScene<Node2D>("res://Scenes/UI/Splash.tscn"));
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
