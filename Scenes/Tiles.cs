using Godot;
using PlatformerWithNoJump;

public partial class Tiles : TileMap
{
    public override void _Ready()
    {
        Modulate = Palette.LightGray;
        base._Ready();
    }
}
