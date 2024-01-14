using Godot;
namespace PlatformerWithNoJump;

public partial class Debug : Node2D, ILevel
{
    [Export]
    public ScreenCamera Camera { get; set; }
    
    [Export]
    public TileMap TileMap { get; set; }

    [Export]
    public DeployedToolsComponent DeployedTools { get; set; }

    private StateTrackerComponent StateTrackerComponent { get; set; }

    private BuildModeComponent BuildModeComponent { get; set; }

    public override void _Ready()
    {
        StateTrackerComponent = GetNode<StateTrackerComponent>("BuildModeUI/StateTrackerComponent");
        BuildModeComponent = GetNode<BuildModeComponent>("BuildModeUI/BuildModeComponent");

        Camera.Chunk = new(0,0);
        base._Ready();
    }

    public override void _Process(double delta)
    {
        QueueRedraw();
        base._Process(delta);
    }

    public override void _Draw()
    {
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
}