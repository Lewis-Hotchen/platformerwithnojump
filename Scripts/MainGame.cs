using Godot;
using PlatformerWithNoJump;

public partial class MainGame : Node2D
{
	[Export]
	public int CurrentLevelScenePathPointer { get; set; }

	private Node2D currentLevel;

	[Export]
	public string[] Levels { get; set; }

	public override void _Ready()
	{
		CurrentLevelScenePathPointer = 0;
		currentLevel = SceneManager.LoadScene<Node2D>(Levels[CurrentLevelScenePathPointer]);
		AddChild(currentLevel);
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
}
