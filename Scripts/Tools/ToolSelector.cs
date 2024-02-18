using System;
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

    [Export]
    public AudioStreamPlayer2D SelectSound { get; set; }

    public Node2D CurrentTool => GetSelectedTool();

    public Tools CurrentToolType => toolsPointer[currentToolPointer];

    private StateTracker states;
    private EventBus eventBus;

    private Dictionary<Tools, Node2D> Tools { get; set; }

    private List<Tools> toolsPointer;

    private Vector2 velocity = new(0, 32);
    private Vector2 desiredPosition = new(0,0);

    private int currentToolPointer = 0;

    private void OnStateChanged(object sender, StateChangedEventArgs e)
    {
        if(e.State == StateTracker.ResourcesState) {
            ToolsList.Clear();
            foreach (var tool in Tools)
            {
                var tex = tool.Value.GetNode<Sprite2D>("Normal").Texture as AtlasTexture;
                ToolsList.AddItem(tool.Key.ToString() + "\nx" + states.Resources[tool.Key], tex);
            }

            QueueRedraw();
        }
    }


    public override void _Ready()
    {
        desiredPosition = Selector.Position; 
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
        
        if(velocity.Y < 0) {
            Selector.Position = new Vector2(Selector.Position.X, Mathf.InverseLerp(Selector.Position.Y, desiredPosition.Y, (float) delta*velocity.Y));
        } else {
            Selector.Position = new Vector2(Selector.Position.X, Mathf.Lerp(Selector.Position.Y, desiredPosition.Y, (float) delta*velocity.Y));
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
                desiredPosition = ToolsList.GetItemRect(currentToolPointer).Position;
                SelectSound.Play();
            }
        }
        else if (Input.IsActionJustPressed("down"))
        {
            if (currentToolPointer < Tools.Count - 1)
            {
                currentToolPointer++;
                desiredPosition = ToolsList.GetItemRect(currentToolPointer).Position;
                SelectSound.Play();
            }
        }
    }

    private Node2D GetSelectedTool()
    {
        return Tools[toolsPointer[currentToolPointer]];
    }
}