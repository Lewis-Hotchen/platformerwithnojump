using System;
namespace PlatformerWithNoJump;

public class ToolSelectedEventArgs : EventArgs
{
    public ToolSelectedEventArgs(string scenePath, ITool tool)
    {
        ScenePath = scenePath;
        Tool = tool;
    }

    public string ScenePath { get; }
    public ITool Tool { get; }
}