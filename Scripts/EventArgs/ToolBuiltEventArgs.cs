using System;
using Godot;
namespace PlatformerWithNoJump;
public class ToolBuiltEventArgs : EventArgs
{
    public ToolBuiltEventArgs(Node2D toolBuilt, Vector2 globalPos)
    {
        ToolBuilt = toolBuilt;
        GlobalPos = globalPos;
    }

    public Node2D ToolBuilt { get; }
    public Vector2 GlobalPos { get; }
}