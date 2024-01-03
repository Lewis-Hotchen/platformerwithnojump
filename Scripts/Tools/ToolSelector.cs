using System;
using Godot;
namespace PlatformerWithNoJump;

public partial class ToolSelector : Node2D
{
    [Export]
    public Springboard Springboard { get; set; }

    [Export]
    public Node2D CurrentTool { get; set; }

    [Export]
    public StateTrackerComponent States { get; set; }

    public event EventHandler<ToolSelectedEventArgs> ToolSelected;

    public override void _Ready()
    {
        CurrentTool = Springboard;
        var selector = GetNode<Control>("Container/SelectorContainer");
        var preview = (ColorRect) Springboard.GetNode<ColorRect>("ColorRect").Duplicate();
        preview.CustomMinimumSize = Springboard.GetNode<ColorRect>("ColorRect").Size*2;
        preview.SizeFlagsHorizontal = Control.SizeFlags.ShrinkCenter;
        preview.SizeFlagsVertical = Control.SizeFlags.ShrinkCenter;
        selector.AddChild(preview);
        base._Ready();
    }

    public override void _Process(double delta)
    {
        if(!States.States["ToolSelectorOpen"]) {
            return;
        }

        if(Input.IsActionJustPressed("ui_select")) {
            ToolSelected?.Invoke(this, new ToolSelectedEventArgs(nameof(Springboard)));
            States.States["ToolSelectorOpen"] = false;
        }

        base._Process(delta);
    }
}

public class ToolSelectedEventArgs : EventArgs
{
    public ToolSelectedEventArgs(string scenePath)
    {
        ScenePath = scenePath;
    }

    public string ScenePath { get; }
}