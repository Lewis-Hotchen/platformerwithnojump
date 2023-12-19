using System;
using Godot;

namespace PlatformerWithNoJump;
public partial class TutorialPart2 : BaseLevel
{
    private bool firstTime = true;
    public override void _Ready()
    {
        OnPlayerFell += PlayerFell;
        GetNode<Timer>("Timer").Timeout += FirstTimeTimeout;
        base._Ready();
    }

    private void FirstTimeTimeout()
    {
        AddChild(SceneManager.LoadScene<Node2D>("res://Scenes/Splash.tscn"));
    }


    private void PlayerFell(object sender, EventArgs e)
    {
        if(firstTime) {
            GetNode<Timer>("Timer").Start();
        }
    }
}
