using Godot;

public partial class NextLevelComponent : Node2D
{
    internal void NextLevel()
    {
        GetParent<Node2D>().GetParent<MainGame>().NextLevel();
    }
}
