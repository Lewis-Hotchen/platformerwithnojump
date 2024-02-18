using System;
using Godot;
namespace PlatformerWithNoJump;

public class ToolBuiltEventArgs : EventArgs
{
    public ToolBuiltEventArgs(Node2D tool, Vector2 globalPosition)
    {
        Tool = tool;
        GlobalPosition = globalPosition;
    }

    public Node2D Tool { get; }
    public Vector2 GlobalPosition { get; }
}