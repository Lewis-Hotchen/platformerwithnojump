using Godot;
using System.Collections.Generic;

public partial class DeployedToolsComponent : Node2D
{
    public List<Node2D> DeployedTools { get; set; }

    public void Reset() {
        DeployedTools.ForEach(x => x.QueueFree());
    }
}