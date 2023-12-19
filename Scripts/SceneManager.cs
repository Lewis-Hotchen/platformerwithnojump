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

    public static BaseLevel GetLevel(string levelScenePath){

       return LoadScene<BaseLevel>(levelScenePath);
    }

    public override void _Process(double delta)
    {
        if (Input.IsActionJustPressed(PauseKey))
        {
            if(!isPaused) {
                GetTree().Paused = true;
                RemoveChild(currentScene);
                AddChild(LoadScene<PauseGame>("res://Scenes/PauseGame.tscn"));
                isPaused = true;
            } else {
                GetNode<PauseGame>("PauseGame").QueueFree();
                AddChild(currentScene);
                isPaused = false;
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