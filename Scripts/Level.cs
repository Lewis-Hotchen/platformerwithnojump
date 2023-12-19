using System;
using Godot;

namespace PlatformerWithNoJump;

public abstract partial class BaseLevel : Node2D {

    public float KillFloorY => GetViewportRect().Size.Y; 

    [Export]
    TileMap TileMap { get; set; }

    [Export]
    Player Player { get; set; }

    private bool hasFallen = false;

    [Export]
    public string NextLevelScenePath { get; set; }
    public event EventHandler OnPlayerFell;

    public override void _PhysicsProcess(double delta)
    {
        if(Player.Position.Y >= KillFloorY) {
            if(hasFallen == false) {
                GetNode<ScreenCamera>("../ScreenCamera").AddTrauma(0.5f);
                OnPlayerFell.Invoke(this, new EventArgs());
                hasFallen = true;
            }
            
        } 

        base._PhysicsProcess(delta);
    }

    public override void _Ready()
    {
        if(Player == null) {
            GetNode<Player>("Player");
        }

        if(TileMap == null) {
            GetNode<TileMap>("Tiles");
        }

        base._Ready();
    }

    internal void NextLevel()
    {
        GetParent<MainGame>().NextLevel();
    }
}