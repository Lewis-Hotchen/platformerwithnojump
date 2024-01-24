using Godot;
using System.Collections.Generic;
namespace PlatformerWithNoJump;

public partial class DeployedToolsComponent : Node2D
{
    private List<Node2D> deployedTools;
    public IEnumerable<Node2D> DeployedTools => deployedTools;

    public override void _Ready()
    {
        deployedTools = new();
        base._Ready();
    }

    public void Add(Node2D tool)
    {
        deployedTools.Add(tool);
    }

    public void Reset()
    {
        deployedTools.ForEach(x => x.QueueFree());
        deployedTools.Clear();
    }
}
