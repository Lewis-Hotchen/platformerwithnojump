using System;
using Godot;
using PlatformerWithNoJump;

public partial class BuildModeComponent : Node2D
{
    public ITool ToolSelected { get; set; }

    [Export]
    public bool IsBuildMode { get; set; }

    [Export]
    public ToolSelector ToolSelector { get; set; }

    [Export]
    public Vector2 Snap { get; set; }

    [Export]
    public StateTrackerComponent StateTrackerComponent { get; set; }

    public event EventHandler<ToolBuiltEventArgs> ToolBuilt;

    private bool isShapeColliding;

    public override void _Ready()
    {
        ToolSelector.ToolSelected += OnToolSelected;
        base._Ready();
    }

    private void OnToolSelected(object sender, ToolSelectedEventArgs e)
    {
        ToolSelected = e.Tool;
        StateTrackerComponent.States["IsBuildMode"] = true;
    }

    public override void _Process(double delta)
    {
        if (StateTrackerComponent.States["IsBuildMode"] && ToolSelected.IsPlaceable)
        {
            if (ToolSelected is Springboard s)
            {
                if (s.GetParent() != this)
                {
                    AddChild(s);
                    s.Visible = true;
                }

                //s.GetNode<Area2D>("Area2D").BodyShapeEntered += ListenForCollision;
                s.GlobalPosition = GetGlobalMousePosition().Snapped(Snap);
                if (IsInBounds(s))
                {
                    s.Modulate = new Color(s.Modulate.R, s.Modulate.B, s.Modulate.G, 1);
                    isShapeColliding = false;
                }
                else
                {
                    s.Modulate = new Color(s.Modulate.R, s.Modulate.B, s.Modulate.G, s.Modulate.A);
                    isShapeColliding = true;
                }

                if (!isShapeColliding && Input.IsActionJustPressed("build"))
                {
                    StateTrackerComponent.States["IsBuildMode"] = false;
                    LockIn(s);
                }
            }
        }

        base._Process(delta);
    }

    private void LockIn(ITool selectedTool)
    {
        var tool = (Node2D) selectedTool;
        var newTool = SceneManager.LoadScene<Node2D>("res://scenes/tools/Springboard.tscn");
        ToolBuilt?.Invoke(this, new ToolBuiltEventArgs(newTool, tool.GlobalPosition));
        tool.QueueFree();
    }

    private static bool IsInBounds(Springboard s)
    {
       return true;
    }

    private void ListenForCollision(Rid bodyRid, Node2D body, long bodyShapeIndex, long localShapeIndex)
    {
        isShapeColliding = true;
    }
}
