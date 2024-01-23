using Godot;
namespace PlatformerWithNoJump;

public partial class Debug : Node2D, ILevel
{
    [Export]
    public ScreenCamera Camera { get; set; }
    

    public override void _Ready()
    {
        Camera.Chunk = new(0,0);
        base._Ready();
    }

    public override void _Process(double delta)
    {
        base._Process(delta);
    }
}