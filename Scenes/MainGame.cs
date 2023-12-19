using Godot;
using System;

public partial class MainGame : Node2D
{
    public override void _Ready()
    {
        GetNode<Camera2D>("ScreenCamera").MakeCurrent();
        base._Ready();
    }
}
