using System.Collections.Generic;
using Godot;
namespace PlatformerWithNoJump;

public partial class ToolSelector : Node2D
{
    [Export]
    public AFP AFP { get; set; }

    [Export]
    public Spring Spring { get; set; }

    [Export]
    public ItemList ToolsList { get; set; }

    [Export]
    public Sprite2D Selector { get; set; }

    public Node2D CurrentTool => GetSelectedTool();

    public Tools SelectedToolType { get; set; }

    private StateTracker states;
    
    private Dictionary<Tools, Node2D> Tools { get; set; }

    private List<Tools> toolsPointer;
    
    private int currentToolPointer = 0;

    public override void _Ready()
    {
        toolsPointer = new List<Tools>(){
            PlatformerWithNoJump.Tools.AFP,
            PlatformerWithNoJump.Tools.Spring
        };

        Tools = new()
        {
            {
                PlatformerWithNoJump.Tools.AFP,
                AFP
            },
            {
                PlatformerWithNoJump.Tools.Spring,
                Spring
            }
        };

        foreach(var tool in Tools) {
            var tex = tool.Value.GetNode<Sprite2D>("Sprite").Texture as AtlasTexture;
            ToolsList.AddItem(tool.Key.ToString(), tex);
        }

        states = GetNode<StateTracker>("/root/StateTracker");
        var selector = GetNode<Control>("ToolsList");

        var preview = CurrentTool;

        base._Ready();
    }

    public override void _Process(double delta)
    {
        if(!states.States["IsBuildMode"]) {
            CyclePointer();
        }

        base._Process(delta);
    }

    private void CyclePointer()
    {
        if(Input.IsActionJustPressed("up")) {
            if(currentToolPointer > 0) {
                currentToolPointer--;
                Selector.Position = ToolsList.GetItemRect(currentToolPointer).Position;
            }
            
        } else if(Input.IsActionJustPressed("down")) {
            if(currentToolPointer < Tools.Count-1) {
                currentToolPointer++;
                Selector.Position = ToolsList.GetItemRect(currentToolPointer).Position;
            } 
        }
    }

    private Node2D GetSelectedTool() {
        return Tools[toolsPointer[currentToolPointer]];
    }
}