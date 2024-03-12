using Godot;
namespace PlatformerWithNoJump;

public partial class Debug : Node2D
{
    [Export]
    public ScreenCamera Camera { get; set; }

    private StateTracker states { get; set; }

    public override void _Ready()
    {
        states = GetNode<StateTracker>("/root/StateTracker");
        states.SetupLevel(new System.Collections.Generic.Dictionary<Tools, ToolResource>() {
            {
                Tools.Spring,
                new ToolResource(){
                    Max = 3,
                    Current = 3
                }
            },
            {
                Tools.AFP,
                new ToolResource(){
                    Max = 3,
                    Current = 3
                }
            }
        }, true, new Tools[] {Tools.Spring, Tools.AFP });
        Camera.Chunk = new(0, 0);
        
        base._Ready();
    }

    public override void _Process(double delta)
    {
        base._Process(delta);
    }
}