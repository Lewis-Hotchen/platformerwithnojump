using Godot;
using PlatformerWithNoJump;
using System;

public partial class L11 : Node2D
{
    private StateTracker states;

    public override void _Ready()
    {
        states = GetNode<StateTracker>("/root/StateTracker");
        states.UpdateResource(Tools.Spring, 3);
        base._Ready();
    }
}