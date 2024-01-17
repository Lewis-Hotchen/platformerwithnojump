using System;
using System.Collections.Generic;
using Godot;
namespace PlatformerWithNoJump;

public partial class ToolSelector : Node2D
{
    [Export]
    public Springboard Springboard { get; set; }

    [Export]
    public Bouncepad Bouncepad { get; set; }

    public ITool CurrentTool => GetSelectedTool();

    public Tools SelectedToolType { get; set; }

    public ColorRect Preview { get; set; }

    private StateTracker states;
    
    private Dictionary<Tools, ITool> Tools { get; set; }

    private List<Tools> toolsPointer;
    
    private int currentToolPointer = 0;

    public override void _Ready()
    {
        toolsPointer = new List<Tools>(){
            PlatformerWithNoJump.Tools.Springboard,
            PlatformerWithNoJump.Tools.Bouncepad
        };

        Tools = new()
        {
            {
                PlatformerWithNoJump.Tools.Springboard,
                Springboard
            },
            {
                PlatformerWithNoJump.Tools.Bouncepad,
                Bouncepad
            }
        };

        states = GetNode<StateTracker>("/root/StateTracker");
        var selector = GetNode<Control>("Container/SelectorContainer");

        Preview = (ColorRect) Springboard.GetNode<ColorRect>("ColorRect").Duplicate();

        Preview.CustomMinimumSize = Springboard.GetNode<ColorRect>("ColorRect").Size*2; 

        Preview.SizeFlagsHorizontal = Control.SizeFlags.ShrinkCenter;
        Preview.SizeFlagsVertical = Control.SizeFlags.ShrinkCenter;

        selector.AddChild(Preview);
        base._Ready();
    }

    public override void _Process(double delta)
    {
        if(!states.States["ToolSelectorOpen"]) {
            return;
        }

        CyclePointer();

        // if(Input.IsActionJustPressed("up") || Input.IsActionJustPressed("down")) {
        //     states.States["ToolSelectorOpen"] = false;
        // }

        base._Process(delta);
    }

    private void CyclePointer()
    {
        if(Input.IsActionJustPressed("up")) {
            if(currentToolPointer > 0) {
                currentToolPointer--;
            }
            
            //Shift our pointer to find index of tool, then from that the correct tool scene from dictionary.
        }
    }

    private ITool GetSelectedTool() {
        return Tools[toolsPointer[currentToolPointer]];
    }

}