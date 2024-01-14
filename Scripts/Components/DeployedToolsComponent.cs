using Godot;
using System;
using System.Collections.Generic;
namespace PlatformerWithNoJump;

public partial class DeployedToolsComponent : Node2D
{
    private List<Node2D> deployedTools;
    public IEnumerable<Node2D> DeployedTools => deployedTools;
    public event EventHandler<ToolAddedEventArgs> ToolAdded;

    public override void _Ready()
    {
        deployedTools = new();
        base._Ready();
    }

    public void Add(Node2D tool, Vector2 pos) {
        if(tool is ITool) {
            deployedTools.Add(tool);
        }

        if(GetParent() is ILevel) {
            GetParent().AddChild(tool);
            tool.Position = pos;
        }
    }

    public void Reset() {
        deployedTools.ForEach(x => x.QueueFree());
        deployedTools.Clear();
    }
}
