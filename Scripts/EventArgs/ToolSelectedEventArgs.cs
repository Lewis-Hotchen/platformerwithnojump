using System;
using Godot;
namespace PlatformerWithNoJump;

public class ToolSelectedEventArgs : EventArgs
{
    public ToolSelectedEventArgs(Control preview, Tools tool)
    {
        ToolPreview = preview;
        Tool = tool;
    }

    public Tools Tool { get; }
    public Control ToolPreview { get; }
}