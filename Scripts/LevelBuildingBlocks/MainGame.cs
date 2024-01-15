using Godot;
using PlatformerWithNoJump;

public partial class MainGame : Node2D
{
	[Export]
	public int CurrentLevelScenePathPointer { get; set; }

	private Node2D currentLevel;

	[Export]
	public string[] Levels { get; set; }

	[Export]
    public StateTrackerComponent StateTrackerComponent { get; set; }

	[Export]
    public BuildModeComponent BuildModeComponent { get; set; }


    public override void _Ready()
	{ 
		CurrentLevelScenePathPointer = 0;
		currentLevel = SceneManager.LoadScene<Node2D>(Levels[CurrentLevelScenePathPointer]);
		GetNode<CanvasLayer>("Game").AddChild(currentLevel);
		GetNode<ColorRect>("ColorRect").Color = Palette.LightGreen;
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

	public override void _Draw()
    {
		var canvas = GetNode<CanvasLayer>("UI");
        if(StateTrackerComponent.States["IsBuildMode"]) {
            Vector2I squareSize = (Vector2I) BuildModeComponent.Snap;
            var rect = GetViewportRect();

            for(int col = 0; col <= rect.Size.X; col += squareSize.X) {
                DrawDashedLine(new Vector2(col, 0), new Vector2(col, rect.Size.Y), Colors.White, 1);
            }

            for(int row = 0; row <= rect.Size.Y; row += squareSize.Y) {
                    DrawDashedLine(new Vector2(0, row), new Vector2(rect.Size.X, row), Colors.White, 1);
            }
        }

        base._Draw();
    }

	public void OnBuildModeUIToolBuilt(Node2D tool) {
		currentLevel.AddChild(tool);
	}
}
