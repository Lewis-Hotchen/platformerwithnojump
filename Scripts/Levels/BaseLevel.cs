using System;
using System.Collections.Generic;
using System.Linq;
using Godot;

namespace PlatformerWithNoJump;

public partial class BaseLevel : Node2D {

    public float KillFloorY => GetViewportRect().Size.Y; 

    [Export]
    public Player Player { get; set; }

    public Vector2 StartPosition { get; set; }

    [Export]
    public string[] States { get; set; }

    public Dictionary<string, bool> StateTracker;

    public event EventHandler<EventArgs> OnPlayerFell;

    public override void _PhysicsProcess(double delta)
    {
        if(Player.Position.Y >= KillFloorY) {
            if(!StateTracker["HasFallen"]) {
                GetParent<Node2D>().GetNode<ScreenCamera>("../ScreenCamera").AddTrauma(0.5f);
                StateTracker["HasFallen"] = true;
                OnPlayerFell?.Invoke(this, new EventArgs());
            }
        }

        base._PhysicsProcess(delta);
    }

    public override void _Ready()
    {
        StateTracker = new()
        {
            { "HasFallen", false }
        };

        if(States != null) {
            foreach(var state in States) {
                StateTracker.Add(state, false);
            }
        }

        Player = GetNode<Player>("Player");
        StartPosition = Player.Position;
        base._Ready();
    }

    public void ResetLevel() {
        Player.Position = StartPosition;
        StateTracker["PlayerHasFallen"] = false;
    }

    internal void NextLevel()
    {
        GetParent<Node2D>().GetParent<MainGame>().NextLevel();
    }
}