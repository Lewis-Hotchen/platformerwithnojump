using Godot;
namespace PlatformerWithNoJump;

public partial class MainGame : Node2D
{
	[Export]
	public int CurrentLevelScenePathPointer { get; set; }

	private Node2D currentLevel;

	[Export]
	public string[] Levels { get; set; }

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
		RemoveChild(currentLevel);
		currentLevel?.QueueFree();
		CurrentLevelScenePathPointer++;
		currentLevel = SceneManager.LoadScene<Node2D>(Levels[CurrentLevelScenePathPointer]);
		AddChild(currentLevel);
	}

    public override void _Process(double delta)
    {
		if(states.States["IsBuildMode"]){
			QueueRedraw();
		}

        base._Process(delta);
    }

    public override void _Draw()
    {
        if(states.States["IsBuildMode"]) {
            Vector2I squareSize = (Vector2I) buildModeComponent.Snap;
            var rect = GetNode<CanvasLayer>("Game").GetViewport().GetVisibleRect().Size;
			rect = new Vector2(rect.X-rect.X*0.1f, rect.Y);
            for(int col = 0; col <= rect.X; col += squareSize.X) {
                DrawDashedLine(new Vector2(col, 0), new Vector2(col, rect.Y), Colors.White, 1, 10);
            }

            for(int row = 0; row <= rect.Y; row += squareSize.Y) {
                DrawDashedLine(new Vector2(0, row), new Vector2(rect.X, row), Colors.White, 1, 10);
            }
        }

        base._Draw();
    }

	public void OnBuildModeUIToolBuilt(Node2D tool, Vector2 globalPosition) {
		currentLevel.AddChild(tool);
		states.States["IsBuildMode"] = false;
		tool.GlobalPosition = globalPosition;
	}
}
