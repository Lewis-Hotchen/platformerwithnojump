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

    public Tools CurrentToolType => toolsPointer[currentToolPointer];

    private StateTracker states;
    private EventBus eventBus;

    private Dictionary<Tools, Node2D> Tools { get; set; }

    private List<Tools> toolsPointer;

    private int currentToolPointer = 0;

    private void OnStateChanged(Node sender, string state, Variant value)
    {
        ToolsList.Clear();
        foreach (var tool in Tools)
        {
            var tex = tool.Value.GetNode<Sprite2D>("Normal").Texture as AtlasTexture;
            ToolsList.AddItem(tool.Key.ToString() + " x" + states.Resources[tool.Key], tex);
        }

        QueueRedraw();
    }


    public override void _Ready()
    {
        states = GetNode<StateTracker>("/root/StateTracker");
        eventBus = GetNode<EventBus>("/root/EventBus");
        toolsPointer = new List<Tools>(states.UnlockedTools);

        Tools = new()
        {
            {
                PlatformerWithNoJump.Tools.Spring,
                Spring
            },
            {
                PlatformerWithNoJump.Tools.AFP,
                AFP
            }
        };

        foreach (var tool in Tools)
        {
            var tex = tool.Value.GetNode<Sprite2D>("Normal").Texture as AtlasTexture;
            ToolsList.AddItem(tool.Key.ToString() + " x" + states.Resources[tool.Key], tex);
        }

        eventBus.StateChanged += OnStateChanged;

        base._Ready();
    }

    public override void _Process(double delta)
    {
        if (!states.GetState("IsBuildMode"))
        {
            CyclePointer();
        }

        base._Process(delta);
    }

    private void CyclePointer()
    {
        if (Input.IsActionJustPressed("up"))
        {
            if (currentToolPointer > 0)
            {
                currentToolPointer--;
                Selector.Position = ToolsList.GetItemRect(currentToolPointer).Position;
            }

        }
        else if (Input.IsActionJustPressed("down"))
        {
            if (currentToolPointer < Tools.Count - 1)
            {
                currentToolPointer++;
                Selector.Position = ToolsList.GetItemRect(currentToolPointer).Position;
            }
        }
    }

    private Node2D GetSelectedTool()
    {
        return Tools[toolsPointer[currentToolPointer]];
    }
}