using Godot;
namespace PlatformerWithNoJump;

public partial class NextLevelComponent : Node2D
{
    internal void NextLevel()
    {
        GetNode<MainGame>("../../../").NextLevel();
    }
}
