using Godot;
using PlatformerWithNoJump;

public partial class MainGame : Node2D
{
    [Export]
    public int CurrentLevelScenePathPointer { get; set; } = 0;

    private BaseLevel currentLevel;

    [Export]
    public string[] Levels { get; set; }

    public override void _Ready()
    {
        GetNode<Camera2D>("ScreenCamera").MakeCurrent();
        currentLevel= SceneManager.GetLevel(Levels[CurrentLevelScenePathPointer]);
        AddChild(currentLevel);
        base._Ready();
    }

    public void NextLevel()
    {
        RemoveChild(currentLevel);
        currentLevel?.QueueFree();
        CurrentLevelScenePathPointer++;
        AddChild(SceneManager.GetLevel(Levels[CurrentLevelScenePathPointer]));
    }
}
