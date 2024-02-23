using System;
using Godot;
namespace PlatformerWithNoJump;

public partial class MainGame : Node2D
{
    [Export]
    public int CurrentLevelScenePathPointer { get; set; }

    public Node2D CurrentLevel { get; private set; }

    [Export]
    public string[] Levels { get; set; }

    [Export]
    public DialogueManagerComponent DialogueManager { get; set; }

    private StateTracker states;
    private EventBus eventBus;
    private BuildModeComponent buildModeComponent;

    public override void _Ready()
    {
        buildModeComponent = GetNode<BuildModeComponent>("UI/HBoxContainer/BuildModeUI/BuildModeComponent");
        states = GetNode<StateTracker>("/root/StateTracker");
        eventBus = GetNode<EventBus>("/root/EventBus");
        CurrentLevelScenePathPointer = 0;
        CurrentLevel = SceneManager.LoadScene<Node2D>(Levels[CurrentLevelScenePathPointer]);
        GetNode<CanvasLayer>("Game").AddChild(CurrentLevel);
        eventBus.ToolBuilt += OnToolBuilt;
        eventBus.ToolFailed += OnToolFailed;
        base._Ready();
    }

    private void OnToolFailed(object sender, ToolFailedEventArgs e)
    {
        switch (e.FailedToolReason)
        {
            case FailedToolReason.RESOURCE_EMPTY:
                DialogueManager.OneShotDialog($"You cannot place anymore {e.Tool}s.");
                break;
            case FailedToolReason.NO_REVERT:
                DialogueManager.OneShotDialog($"You cannot revert anymore {e.Tool}s.");
                break;
        }
    }


    public void NextLevel()
    {
        var oldLevel = Levels[CurrentLevelScenePathPointer];
        GetNode<CanvasLayer>("Game").RemoveChild(CurrentLevel);
        CurrentLevel?.QueueFree();
        CurrentLevelScenePathPointer++;
        CurrentLevel = SceneManager.LoadScene<Node2D>(Levels[CurrentLevelScenePathPointer]);
        var newLevel = Levels[CurrentLevelScenePathPointer];
        GetNode<CanvasLayer>("Game").AddChild(CurrentLevel);
        eventBus.RaiseEvent(nameof(EventBus.LevelChanged), this, new LevelChangedEventArgs(oldLevel, newLevel));
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

    public void OnToolBuilt(object sender, ToolBuiltEventArgs e)
    {
        CurrentLevel.AddChild(e.Tool);
        states.SetState("IsBuildMode", false);
        e.Tool.GlobalPosition = e.GlobalPosition;
        var toolComp = e.Tool.GetNode<ToolComponent>("ToolComponent");
        toolComp.CanFall = true;
        toolComp.SetDirection(e.Tool.RotationDegrees);
        toolComp.IsActive = true;
    }

    protected override void Dispose(bool disposing)
    {
        if(disposing) {
            eventBus.ToolBuilt -= OnToolBuilt;
            eventBus.ToolFailed -= OnToolFailed;
        }

        base.Dispose(disposing);
    }
}
