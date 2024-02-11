using System;
using Godot;
namespace PlatformerWithNoJump;

public partial class BuildModeComponent : Node2D
{
    [Export]
    public Node2D DefaultToolPreview { get; set; }

    public Node2D Preview { get; set; }

    public Tools ToolSelected { get; set; }

    [Export]
    public bool IsBuildMode { get; set; }

    [Export]
    public Vector2 Snap { get; set; }

    private StateTracker states;

    public event EventHandler<ToolBuiltEventArgs> ToolBuilt;

    private bool isShapeColliding;

    public override void _Ready()
    {
        states = GetNode<StateTracker>("/root/StateTracker");
        base._Ready();
    }

    public void StartBuild(Node2D toolPreivew)
    {
        Preview = (Node2D)toolPreivew.Duplicate();
        states.SetState("IsBuildMode", true);
    }

    public override void _Process(double delta)
    {
        if (states.GetState("IsBuildMode"))
        {
            if (!IsInstanceValid(Preview) || Preview == null)
            {
                Preview = DefaultToolPreview;
            }

            if (IsInstanceValid(Preview) && Preview != null)
            {
                if (Preview.GetParent() != this)
                {
                    AddChild(Preview);
                    Preview.Visible = true;
                    Preview.GlobalPosition = GetCentreofScreen().Snapped(Snap);
                }

                if (!isShapeColliding && Input.IsActionJustPressed("build"))
                {
                    states.SetState("IsBuildMode", false);
                    LockIn();
                    return;
                }

                //s.GetNode<Area2D>("Area2D").BodyShapeEntered += ListenForCollision;
                Vector2 direction = GetDirection();

                Preview.GlobalPosition += direction * Snap;

                if (IsInBounds(Preview))
                {
                    Preview.Modulate = new Color(Preview.Modulate.R, Preview.Modulate.B, Preview.Modulate.G, 1);
                    isShapeColliding = false;
                }
                else
                {
                    Preview.Modulate = new Color(Preview.Modulate.R, Preview.Modulate.B, Preview.Modulate.G, Preview.Modulate.A);
                    isShapeColliding = true;
                }
            }
        }
        else if (!states.GetState("IsBuildMode") && Node2D.IsInstanceValid(Preview))
        {
            Preview.QueueFree();
        }

        base._Process(delta);
    }

    private static Vector2 GetDirection()
    {
        if (Input.IsActionJustPressed("up"))
            return Vector2.Up;
        if (Input.IsActionJustPressed("down"))
            return Vector2.Down;
        if (Input.IsActionJustPressed("right"))
            return Vector2.Right;
        if (Input.IsActionJustPressed("left"))
            return Vector2.Left;

        return Vector2.Zero;
    }

    private static Vector2 GetCentreofScreen()
    {
        return new Vector2(1280 / 2, 680 / 2);
    }

    private void LockIn()
    {
        var tool = Preview.GetNode<ToolComponent>("ToolComponent").ToolType;
        var newTool = SceneManager.LoadScene<Node2D>($"res://scenes/tools/{tool}.tscn"); // Replace with enum logic to pair up enum with tool scene.
        ToolBuilt?.Invoke(this, new ToolBuiltEventArgs(newTool, Preview.GlobalPosition));
        Preview.QueueFree();
    }

    private static bool IsInBounds(Node2D tool)
    {
        return true;
    }

    private void ListenForCollision(Rid bodyRid, Node2D body, long bodyShapeIndex, long localShapeIndex)
    {
        isShapeColliding = true;
    }
}
