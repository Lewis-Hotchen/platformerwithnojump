using System;
using Godot;

namespace PlatformerWithNoJump;
public partial class TutorialPart3 : Node2D
{
    [Export]
    public BaseLevel Level { get; set; }

    public override void _Ready()
    {
        Level.OnPlayerFell += PlayerFell;
        GetNode<Timer>("Timer").Timeout += FirstTimeTimeout;
        GetNode<Timer>("SplashScreenTimeout").Timeout += SplashScreenTimeout;

        base._Ready();
    }

    private void SplashScreenTimeout()
    {
        GetNode<Node2D>("Splash").QueueFree();
        Level.StateTracker["HasCompletedTutorial"] = true;
        Level.ResetLevel();
    }

    private void FirstTimeTimeout()
    {
        AddChild(SceneManager.LoadScene<Node2D>("res://Scenes/Splash.tscn"));
        GetNode<Timer>("SplashScreenTimeout").Start();
    }

    private void PlayerFell(object sender, EventArgs e)
    {
        if(!Level.StateTracker["HasCompletedTutorial"]) {
            GetNode<Timer>("Timer").Start();
        } else {
            Level.ResetLevel();
        }
    }
}
