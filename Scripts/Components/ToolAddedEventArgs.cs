using Godot;
using System;
namespace PlatformerWithNoJump;

public class ToolAddedEventArgs : EventArgs
{
    public ToolAddedEventArgs(Node2D toolAdded)
    {
        ToolAdded = toolAdded;
    }

    public Node2D ToolAdded { get; }
}