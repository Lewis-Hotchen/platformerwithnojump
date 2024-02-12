using Godot;

namespace PlatformerWithNoJump;


public partial class SceneManager : Node
{

    [Export]
    public string PauseKey { get; set; }

    private Node currentScene;

    [Export]
    public string MainGameWindowPath { get; set; }

    [Export]
    public string MainMenuPath { get; set; }

    private bool isPaused;

    public override void _Ready()
    {
        ProcessMode = ProcessModeEnum.Always;
        AddScene<Node>(MainMenuPath);
        base._Ready();
    }

    public void AddScene<T>(string path) where T : class
    {
        var scene = LoadScene<T>(path);
        if (scene is Node n)
        {
            AddChild(n);
            n.ProcessMode = ProcessModeEnum.Pausable;
        }
    }

    public void SwitchScene<T>(string path) where T : class
    {
        currentScene?.QueueFree();
        var scene = LoadScene<T>(path);
        if (scene is Node n)
        {
            AddChild(n);
            currentScene = n;
            currentScene.ProcessMode = ProcessModeEnum.Pausable;
        }
    }

    public override void _Process(double delta)
    {
        if (Input.IsActionJustPressed(PauseKey))
        {
            if (!GetTree().Paused)
            {
                GetTree().Paused = true;
                RemoveChild(currentScene);
                AddChild(LoadScene<Node2D>("res://Scenes/PauseGame.tscn"));
            }
            else
            {
                GetNode<Node2D>("PauseGame").QueueFree();
                AddChild(currentScene);
            }
        }

        base._Process(delta);
    }

    public static T LoadScene<T>(string path) where T : class
    {
        var packedGame = GD.Load<PackedScene>(path);
        return packedGame.Instantiate<T>();
    }
}
