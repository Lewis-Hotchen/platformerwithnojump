using System;
using Godot;
namespace PlatformerWithNoJump;

public partial class MainGame : Node2D
{
    [Export]
    public int CurrentLevelScenePathPointer { get; set; }

    private Node2D currentLevel;

    [Export]
    public string[] Levels { get; set; }

    [Export]
    public DialogueManagerComponent DialogueManager;

    private StateTracker states;
    private BuildModeComponent buildModeComponent;

    public override void _Ready()
    {
        buildModeComponent = GetNode<BuildModeComponent>("UI/HBoxContainer/BuildModeUI/BuildModeComponent");
        states = GetNode<StateTracker>("/root/StateTracker");
        CurrentLevelScenePathPointer = 0;
        currentLevel = SceneManager.LoadScene<Node2D>(Levels[CurrentLevelScenePathPointer]);
        GetNode<CanvasLayer>("Game").AddChild(currentLevel);
        base._Ready();
    }

    public void NextLevel()
    {
        GetNode<CanvasLayer>("Game").RemoveChild(currentLevel);
        currentLevel?.QueueFree();
        CurrentLevelScenePathPointer++;
        currentLevel = SceneManager.LoadScene<Node2D>(Levels[CurrentLevelScenePathPointer]);
        GetNode<CanvasLayer>("Game").AddChild(currentLevel);
    }

    public override void _Process(double delta)
    {
        if (states.GetState("IsBuildMode"))
        {
            QueueRedraw();
        }

        base._Process(delta);
    }

    public override void _Draw()
    {
        var viewPortRect = GetNode<CanvasLayer>("Game").GetViewport().GetVisibleRect().Size;

        if (states.GetState("IsBuildMode"))
        {
            Vector2I squareSize = (Vector2I)buildModeComponent.Snap;
            var gameRect = new Vector2(viewPortRect.X - viewPortRect.X * 0.1f, viewPortRect.Y);
            for (int col = 0; col <= gameRect.X; col += squareSize.X)
            {
                DrawDashedLine(new Vector2(col, 0), new Vector2(col, gameRect.Y), new Color(148, 148, 148, 0.5f), 1, 3);
            }

            for (int row = 0; row <= gameRect.Y; row += squareSize.Y)
            {
                DrawDashedLine(new Vector2(0, row), new Vector2(gameRect.X, row), new Color(148, 148, 148, 0.5f), 1, 3);
            }
        }

        var containerRect = new Rect2I(new Vector2I(Convert.ToInt32(viewPortRect.X - 96), 0), new Vector2I(96, Convert.ToInt32(viewPortRect.Y)));
        DrawRect(containerRect, Colors.White, false, 1);

        base._Draw();
    }

    public void OnBuildModeUIToolBuilt(Node2D tool, Vector2 globalPosition)
    {
        currentLevel.AddChild(tool);
        states.SetState("IsBuildMode", false);
        tool.GlobalPosition = globalPosition;
    }
}
