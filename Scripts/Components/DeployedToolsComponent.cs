using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
namespace PlatformerWithNoJump;

public partial class DeployedToolsComponent : Node2D, IDisposable
{
    private Stack<Node2D> deployedTools;
    public IEnumerable<Node2D> DeployedTools => deployedTools;
    private EventBus eventBus;

    public override void _Ready()
    {
        eventBus = GetNode<EventBus>("/root/EventBus");
        eventBus.LevelChanged += OnLevelChanged;
        deployedTools = new();
        base._Ready();
    }

    private void OnLevelChanged(object sender, LevelChangedEventArgs e)
    {
        Reset();
    }


    public void Add(Node2D tool)
    {
        deployedTools.Push(tool);
    }

    public Tools RemoveLast() {
        if(!deployedTools.Any()) {
            eventBus.RaiseEvent(nameof(EventBus.ToolFailed), this, new ToolFailedEventArgs(Tools.None, FailedToolReason.NO_REVERT));
            return Tools.None;
        }

        var tool = deployedTools.Pop();
        var type = tool.GetNode<ToolComponent>("ToolComponent").ToolType;
        tool.QueueFree();
        return type;
    }

    public void Reset()
    {
        foreach(var tool in deployedTools) {
            tool.QueueFree();
        }

        deployedTools.Clear();
    }

    protected override void Dispose(bool disposing)
    {
        if(disposing) {
            eventBus.LevelChanged -= OnLevelChanged;
        }
        
        base.Dispose(disposing);
    }

}
